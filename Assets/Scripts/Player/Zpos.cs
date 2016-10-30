using UnityEngine;
using System.Collections;

public class Zpos : MonoBehaviour {

    public float offset;

	// Update is called once per frame
	void Update () {
        transform.position = new Vector3(transform.position.x, transform.position.y, offset + transform.position.y);
    }
}
