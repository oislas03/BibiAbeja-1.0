// Add this script to a GameObject. The Start() function fetches an
// image from the documentation site.  It is then applied as the
// texture on the GameObject.
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;




public class controlGaleria : MonoBehaviour
{

    public changeImg[] imagenes = new changeImg[4];

    List<string> nombreImagenes = new List<string>();
    public string[] paths = new string[4];

    List<Imagen> imagenesObj = new List<Imagen>();

    string nombreActivo = "";



    void Start()
    {
        //EstadoJuego.estadoJuego.setUsuario(1);
        imagenesObj = EstadoJuego.estadoJuego.cargarImagenesPorNino();



        //Debug.Log();
        foreach (Imagen img in imagenesObj)
        {
            if (!nombreImagenes.Contains(img.nombre))
            {
                nombreImagenes.Add(img.nombre);
            }
        }

        this.nombreActivo = nombreImagenes[0];

        ponerImagen();


    }


    public void CambiarImagenIzq() {

        int i = nombreImagenes.IndexOf(nombreActivo);

        if ((i + 1) < nombreImagenes.Count) {
            this.nombreActivo = nombreImagenes[i + 1];

            ponerImagen();

        }
    }



    public void CambiarImagenDerecha ()
    {

        int i = nombreImagenes.IndexOf(nombreActivo);

        if ((i ) < nombreImagenes.Count&& i>0)
        {
            this.nombreActivo = nombreImagenes[i - 1];

            ponerImagen();

        }
    } 

    public void ponerImagen() {
        AudioClip sonido;

        GameObject.Find("nNino").GetComponent<Text>().text = this.nombreActivo;
        sonido = Resources.Load<AudioClip>("Sonidos/Figuras/" + this.nombreActivo);
        GetComponent<AudioSource>().PlayOneShot(sonido);

        imagenes[0].colocarImagen("");
        imagenes[1].colocarImagen("");
        imagenes[2].colocarImagen("");
        imagenes[3].colocarImagen("");



        foreach (Imagen img in imagenesObj)
        {

            if (img.nombre.Equals(nombreActivo))
            {

                string imgpath = img.path;
                switch (img.numPart)
                {
                    case 1:
                        imagenes[0].colocarImagen(img.path);
                        break;
                    case 2:
                        imagenes[1].colocarImagen(img.path);
                        break;
                    case 3:
                        imagenes[2].colocarImagen(img.path);
                        break;
                    case 4:
                        imagenes[3].colocarImagen(img.path);
                        break;
                }

            }

        }


    }
}