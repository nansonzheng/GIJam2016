using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turf : MonoBehaviour {

	public IList<Collider2D> collisions = new List<Collider2D>(); 

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		//This is supposed to be the block
		if(col.gameObject.CompareTag("Block")){
			Color color = col.gameObject.GetComponent<SpriteRenderer> ().color;
			Debug.Log (color.GetHashCode());
			if(!(color == Color.white))
				this.GetComponent<SpriteRenderer> ().color = color;
		}
	}
}
