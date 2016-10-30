using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blockBehaviour : MonoBehaviour {

	private 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D col){
		//This is supposed to be the Player (but its gonna be anything)
		if (col.gameObject.CompareTag ("Player")) {
			Color color = col.gameObject.GetComponent<SpriteRenderer> ().color;

			if (color != this.GetComponent<SpriteRenderer> ().color) {
				SpriteRenderer[] sr = this.GetComponentsInChildren<SpriteRenderer> ();
				foreach (SpriteRenderer obj in sr) {
					obj.color = color;
				}
			}
		}
	}
}
