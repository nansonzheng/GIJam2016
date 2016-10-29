using UnityEngine;
using System.Collections;

public class PushedBehaviour : MonoBehaviour {

    Rigidbody2D rb;
    GameObject pushedBy;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        pushedBy = null;
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (pushedBy == null)
        {
            pushedBy = col.gameObject;
        }
    }

    void OnCollisionExit2D(Collision2D col)
    {
        if(col.gameObject == pushedBy)
        {
            pushedBy = null;
        }
    }
}
