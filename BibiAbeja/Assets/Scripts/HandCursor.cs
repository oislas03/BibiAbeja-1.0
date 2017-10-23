using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
//using vJoyInterfaceWrap;
using System.Threading;
using UnityEngine.SceneManagement;

public class HandCursor : MonoBehaviour
{

    private PXCMSenseManager sm;
    private PXCMHandCursorModule cursorModule;
    private PXCMCursorConfiguration cursorConfig;
    private PXCMSession session;
    private PXCMCursorData cursorData;
    private PXCMCursorData.GestureData gestureData;
    private PXCMPoint3DF32 adaptivePoints;
    private PXCMPoint3DF32 coordinates2d;


    //public List<Rect> linea;
    //public Material punto = new Material(Shader.Find("Unlit/Color"));
    //public Texture2D puntoImage;
    public Texture2D cursorImage;
    public Vector3 mousePos;
    public bool click;
    public static HandCursor me;
    public delegate void JugadorEmpiezaDibujar(String id);
    public static event JugadorEmpiezaDibujar Contacto;
    public Vector3 v3 = new Vector3();
    public bool primeraVez = true;

    private void Awake()
    {
        if (me == null)
        {
            me = this;

        }
        else if (me != this)
        {

            Destroy(gameObject);
        }

    }


    private void Start()
    {
        //linea = new List<Rect>();
        ConfigureRealSense();
        Update();
    }


    void OnMouseEnter()
    {
        UnityEngine.Cursor.visible = true;

    }

    public void ConfigureRealSense()
    {
        // Create an instance of the SenseManager
        sm = PXCMSenseManager.CreateInstance();

        // Enable cursor tracking
        sm.EnableHandCursor();

        // Create a session of the RealSense
        session = PXCMSession.CreateInstance();

        // Get an instance of the hand cursor module
        cursorModule = sm.QueryHandCursor();
        // Get an instance of the cursor configuration
        cursorConfig = cursorModule.CreateActiveConfiguration();

        // Make configuration changes and apply them
        cursorConfig.EnableEngagement(true);
        cursorConfig.EnableAllGestures();
        cursorConfig.EnableAllAlerts();
        cursorConfig.ApplyChanges();

        // Initialize the SenseManager pipeline
        sm.Init();
    }

    private void Update()
    {
        StartCoroutine("toUpdate");
    }

    IEnumerator toUpdate()
    {
        bool handInRange = true;

        if (sm.AcquireFrame(true).IsSuccessful())
        {
            // Hand and cursor tracking 
            cursorData = cursorModule.CreateOutput();
            adaptivePoints = new PXCMPoint3DF32();
            coordinates2d = new PXCMPoint3DF32();
            PXCMCursorData.BodySideType bodySide;

            cursorData.Update();

            // Check if alert data has fired
            for (int i = 0; i < cursorData.QueryFiredAlertsNumber(); i++)
            {
                PXCMCursorData.AlertData alertData;
                cursorData.QueryFiredAlertData(i, out alertData);

                if ((alertData.label == PXCMCursorData.AlertType.CURSOR_NOT_DETECTED) ||
                    (alertData.label == PXCMCursorData.AlertType.CURSOR_DISENGAGED) ||
                    (alertData.label == PXCMCursorData.AlertType.CURSOR_OUT_OF_BORDERS))
                {
                    handInRange = false;
                }
                else
                {
                    handInRange = true;
                }
            }


            if (cursorData.IsGestureFired(PXCMCursorData.GestureType.CURSOR_CLICK, out gestureData))
            {
                click = true;
            }

            // Track hand cursor if it's within range
            int detectedHands = cursorData.QueryNumberOfCursors();

            if (detectedHands > 0)
            {
                // Retrieve the cursor data by order-based index
                PXCMCursorData.ICursor iCursor;
                cursorData.QueryCursorData(PXCMCursorData.AccessOrderType.ACCESS_ORDER_NEAR_TO_FAR,
                                           0,
                                           out iCursor);

                adaptivePoints = iCursor.QueryAdaptivePoint();

                // Retrieve controlling body side (i.e., left or right hand)
                bodySide = iCursor.QueryBodySide();

                Debug.Log("Resolución actual: " + Screen.currentResolution.height + ", " + Screen.currentResolution.width);

                coordinates2d.x = (adaptivePoints.x * Screen.currentResolution.width);
                coordinates2d.y = (adaptivePoints.y * Screen.currentResolution.height);

                mousePos = new Vector3(coordinates2d.x, coordinates2d.y, 20);
                //mousePos = new Vector3(coordinates2d.x, coordinates2d.y, 20);

                v3 = Camera.main.WorldToScreenPoint(mousePos);
            }
            else
            {
                bodySide = PXCMCursorData.BodySideType.BODY_SIDE_UNKNOWN;
            }

            // Resume next frame processing
            cursorData.Dispose();
            sm.ReleaseFrame();
        }
        yield return null;
    }

    Ray ray;
    RaycastHit hit;

    void OnGUI()
    {
        mousePos = Camera.main.ScreenToWorldPoint(v3);

        // Cuadrado y posición en la pantalla que ayuda a dibujar el lápiz.
        Rect posa = new Rect(mousePos.x, Screen.currentResolution.height - mousePos.y, cursorImage.width, cursorImage.height);
        GUI.Label(posa, cursorImage);



        ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "salir")
            {
                SceneManager.LoadScene("eligeTema");
            }
            else if (hit.collider.tag == "punto")
            {
                //Debug.Log("Ahora sí");
                var nombre = hit.collider.gameObject.name;
                nombre = nombre.Substring(5);
                // Revisa que haya un objeto suscrito al evento
                if (Contacto != null)
                {
                    // llama al evento del observador
                    Contacto(nombre);
                }

            }
            if (hit.collider.tag == "areaDibujable")
            {
                GameObject.Find("DrawLine").GetComponent<DrawLine>().Dibujar(mousePos);
            }
        }
    }

    public PXCMHandCursorModule getCursorModule()
    {
        return cursorModule;
    }

    public Vector3 getRealsensePosition()
    {
        return mousePos;
    }

    public PXCMSenseManager getSenseManager()
    {
        return sm;
    }

    public void setSenseManager(PXCMSenseManager sm)
    {
        this.sm = sm;
    }

    public void disposeSenseManager()
    {
        sm.Dispose();
    }

    public PXCMPoint3DF32 getCoordenadasRealSense()
    {
        return coordinates2d;
    }
}