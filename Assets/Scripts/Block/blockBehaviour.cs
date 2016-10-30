using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blockBehaviour : MonoBehaviour {

	private IList<Collision2D> playerCollisions = new List<Collision2D>();
	private IList<Collider2D> platformTriggers = new List<Collider2D> ();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//Remove all dead player touches
		IList<Collision2D> toRemove = new List<Collision2D>();
		for(int i = 0;i < playerCollisions.Count; i++) {
			if (playerCollisions[i].gameObject == null || !playerCollisions[i].gameObject.GetComponent<Controls> ().alive.Value) {
				toRemove.Add (playerCollisions[i]);
			}
		}

		foreach (Collision2D c in toRemove) {
			playerCollisions.Remove(c);
		}
	}

	void OnCollisionEnter2D(Collision2D col){
		//This is supposed to be the Player (but its gonna be anything)
		if (col.gameObject.CompareTag ("Player")) {
			Color color = col.gameObject.GetComponent<SpriteRenderer> ().color;

			//Update Block Color
			if (color != this.GetComponent<SpriteRenderer> ().color && playerCollisions.Count == 0) {
				SpriteRenderer[] sr = this.GetComponentsInChildren<SpriteRenderer> ();
				foreach (SpriteRenderer obj in sr) {
					obj.color = color;
				}
			}
			//Change in color, change blocks below
			changeCurrentPlatforms();
			playerCollisions.Add(col);
		}
	}

	void OnCollisionExit2D(Collision2D col){
		if (col.gameObject.CompareTag ("Player")) {
			for(int i = 0; i < playerCollisions.Count; i++){
				if (playerCollisions [i].gameObject == col.gameObject){
					playerCollisions.RemoveAt(i);

					//Reupdate Color
					if(playerCollisions.Count >= 1){
						SpriteRenderer[] sr = this.GetComponentsInChildren<SpriteRenderer> ();
						foreach (SpriteRenderer obj in sr) {
							obj.color = playerCollisions[0].gameObject.GetComponent<SpriteRenderer> ().color;
						}
					}	
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		//Add the list of currently touching platforms
		if(col.gameObject.CompareTag("Platform")){
			platformTriggers.Add (col);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		//Remmove the leaving platform
		if(col.gameObject.CompareTag("Platform")){
			for (int i = 0; i < platformTriggers.Count; i++) {
				if (platformTriggers [i].gameObject == col.gameObject) {
					platformTriggers.RemoveAt (i);
				}
			}
		}
	}

	void changeCurrentPlatforms(){
		foreach (Collider2D c in platformTriggers) {
			c.gameObject.GetComponent<turf> ().notifyChange(GetComponent<SpriteRenderer>().color);
		}
	}
}
