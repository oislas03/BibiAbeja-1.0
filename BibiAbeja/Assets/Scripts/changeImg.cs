using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeImg : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void colocarImagen(string path)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load(path, typeof(Sprite)) as Sprite;
    }
}
