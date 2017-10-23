using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

public class controlImagen : MonoBehaviour
{
    public changeImg[] imagenes = new changeImg[4];
    List<Imagen> imgDesbloqueadas = new List<Imagen>();
    Imagen[] imagenActiva = new Imagen[4];
    int[] index = new int[4];
    //float timeLeft = 3;




    // Use this for initialization
    void Start()

    {


        imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra();

        Debug.Log(imgDesbloqueadas.Count);
        if (imgDesbloqueadas.Count != 4)
        {
            Debug.Log("sigo aqui?");
            EstadoJuego.estadoJuego.guardarImagenDesbloqueada();
            EstadoJuego.estadoJuego.guardarImagenDesbloqueada();
            imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra();

        }
        else
        {
            List<string> palabrasTemas = EstadoJuego.estadoJuego.obtenerPalabrasTema();


            foreach (string pb in palabrasTemas)
            {

                imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra(pb);
                Debug.Log(imgDesbloqueadas.ToString());

                if (imgDesbloqueadas.Count != 4)
                {
                    EstadoJuego.estadoJuego.guardarImagenDesbloqueada(pb);
                    EstadoJuego.estadoJuego.guardarImagenDesbloqueada(pb);
                    imgDesbloqueadas = EstadoJuego.estadoJuego.cargarImagenesDesbloqueadasporPalabra(pb);
                    break;

                }
            }

        }
     


        int i = 0;
        foreach (Imagen img in imgDesbloqueadas)
        {
            if (i < imgDesbloqueadas.Count - 2)
            {
                imagenes[i].colocarImagen(img.path);
            }
            else
            {
                index[i] = i;
                this.imagenActiva[i] = img;
            }
            i++;
        }

        Invoke("ponerImagen", 1);



    }

    // Update is called once per frame

    void update()
    {

    }

    public void ponerImagen()
    {

        for (int i = 0; i < this.imagenActiva.Length; i++)
        {

            if (imagenActiva[i] != null)
            {
                this.imagenes[index[i]].colocarImagen(imagenActiva[i].path);

            }

        }

        GameObject.Find("Imagen1").GetComponent<AudioSource>().Play();


    }



}
