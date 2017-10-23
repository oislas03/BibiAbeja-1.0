using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class dropDownHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    public void seleccionarPalabra(string nombre) {
        int valor = GameObject.Find(nombre).GetComponent<Dropdown>().value;

        string valorReal = GameObject.Find(nombre).GetComponent<Dropdown>().options[valor].text;

        EstadoJuego.estadoJuego.setPalabra(valorReal);

        switch (nombre)
        {
            case "btnFiguras":
                EstadoJuego.estadoJuego.setTema("geometria");
                break;
            case "btnAnimales":
                EstadoJuego.estadoJuego.setTema("animales");
                break;
            case "btnMediosTransporte":
                EstadoJuego.estadoJuego.setTema("frutas");
                break;
            case "btnNaturaleza":
                EstadoJuego.estadoJuego.setTema("naturaleza");
                break;
            case "btnHogar":
                EstadoJuego.estadoJuego.setTema("hogar");
                break;
            case "btnFrutas":
                EstadoJuego.estadoJuego.setTema("frutas");
                break;
   
        }
        SceneManager.LoadScene("paso 1");
    }

    private void OneChanged(int newPosition)
    {

        string realValue = GameObject.Find("btnFiguras").GetComponent<Dropdown>().options[newPosition].text;
        Debug.Log("que paso aqi?");


        // realValue is the integer value associated with this key index
        // do whatever you need to do with it here
    }
}
