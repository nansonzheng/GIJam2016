using UnityEngine;
using System.Collections;

public class Zpos : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y);
    }
}
