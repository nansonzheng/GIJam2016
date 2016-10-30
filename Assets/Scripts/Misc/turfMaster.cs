using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turfMaster : MonoBehaviour {

	private IList<GameObject> turf = new List<GameObject>(); 

	// Use this for initialization
	void Start () {
		foreach (Transform child in transform) {
			turf.Add (child.gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	int countColor(Color color){
		int counter = 0;
		foreach (GameObject g in turf) {
			if (g.GetComponent<SpriteRenderer> ().color == color)
				counter++;
		}

		return counter;
	}
}
