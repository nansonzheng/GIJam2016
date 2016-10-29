using UnityEngine;
using System.Collections;

public class PushedBehaviour : MonoBehaviour {

    Rigidbody2D rb;
    GameObject pushedBy;
    public bool debug;
    public float debugMoveSpd;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        pushedBy = null;
	}
	
	// Update is called once per frame
	void Update () {
	    if (debug)
        {
            Vector2 move = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            if (move != Vector2.zero)
            rb.velocity = move;
        }
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
