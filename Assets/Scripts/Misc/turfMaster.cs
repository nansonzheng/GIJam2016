using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class turfMaster : MonoBehaviour {

	private IList<GameObject> turf = new List<GameObject>();

    public Color[] colors = new Color[4] {
        new Color(121/255, 1, 1),
        new Color(99/255, 1, 100/255),
        new Color(1, 125, 219),
        new Color(235/255, 1, 52/255),

    };

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

    public int winner()
    {
        int win = 0;
        foreach (Color c in colors)
        {
            int temp = countColor(c);
            if (temp > win)
            {
                win = temp;
            }
        }
        return win;
    }
}
