using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class blockBehaviour : MonoBehaviour {

	private IList<Collision2D> playerCollisions = new List<Collision2D>();
	private int listLength = 0;

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

			if (color != this.GetComponent<SpriteRenderer> ().color && listLength == 0) {
				SpriteRenderer[] sr = this.GetComponentsInChildren<SpriteRenderer> ();
				foreach (SpriteRenderer obj in sr) {
					obj.color = color;
				}
			}

			playerCollisions.Add(col);
			++listLength;
		}
	}

	void OnCollisionExit2D(Collision2D col){
		for(int i = 0; i < listLength; i++){
			if (playerCollisions[i].gameObject.GetComponent<Controls>().alive.Value || playerCollisions[i].gameObject == col.gameObject) {
				playerCollisions.RemoveAt(i);
				--listLength;

				//Reupdate Color
				if(listLength >= 1){
					SpriteRenderer[] sr = this.GetComponentsInChildren<SpriteRenderer> ();
					foreach (SpriteRenderer obj in sr) {
						obj.color = playerCollisions[0].gameObject.GetComponent<SpriteRenderer> ().color;
					}
				}

				break;
			}
		}
	}
}
