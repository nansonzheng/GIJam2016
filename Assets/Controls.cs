using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    public enum Player {
        Player1,
        Player2
    }

    public Player playerNum;
    public float speed;
    public float pushingMultiplier;
    public bool isPushing;
    Rigidbody2D rb, pushed;
    Vector2 attachNormal;

    Vector2 direction;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
	    switch (playerNum)
        {
            case Player.Player1:
                direction.x = Input.GetAxis("Horizontal");
                direction.y = Input.GetAxis("Vertical");
                break;
            case Player.Player2:
                direction.x = Input.GetAxis("Horizontal2");
                direction.y = Input.GetAxis("Vertical2");
                break;
        }
        rb.velocity = direction * speed;
        /*
        if (!isPushing)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            // Determine if moving away
            float angleDifference = Mathf.Abs(Vector2.Angle(attachNormal, direction));
            Debug.Log("AngleDifference = " + angleDifference);
            if (angleDifference < 90)
                rb.velocity = direction * speed;
            else
            {
                rb.velocity = pushed.velocity = direction * speed * pushingMultiplier;
            }
        }
        */            
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (!isPushing) {
            // TODO: Check collided object is wall or playblock
            isPushing = true;
            pushed = col.rigidbody;
            attachNormal = col.contacts[0].normal;
            Debug.Log(attachNormal);
        }
        // Else, probably being squished
    }

    void OnCollisionExit2D() {
        if (isPushing)
        {
            isPushing = false;
            pushed = null;
            attachNormal = Vector2.zero;
        }
    }
}
