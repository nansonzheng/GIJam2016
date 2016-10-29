using UnityEngine;
using System.Collections;

public class turf : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		//This is supposed to be the block (but its gonna be anything)
		Color color = col.gameObject.GetComponent<SpriteRenderer> ().color;
		this.GetComponent<SpriteRenderer> ().color = color;
	}
}
