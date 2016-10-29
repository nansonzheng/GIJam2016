using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {

    public enum Player {
        Player1,
        Player2,
        Player3
    }

    public Player playerNum;
    public float speed;
    public float pushingMultiplierSide, pushingMultiplierForward;
    public bool isPushing;
    public float ctrlThresh;
    Rigidbody2D rb, pushed;
    Vector2 attachNormal;
    public Vector2 scale;

    Vector2 direction;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        scale = Vector2.one;
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
            case Player.Player3:
                direction.x = Input.GetAxis("Horizontal3");
                direction.y = Input.GetAxis("Vertical3");
                break;
        }
        //rb.velocity = direction * speed;
        
        if (!isPushing)
        {
            rb.velocity = direction * speed;
        }
        else
        {
            // Determine if movement is to push block
            float pushDiff = Mathf.Abs(Vector2.Angle(attachNormal, direction));
            Debug.Log("pushDifference = " + pushDiff);
            // Determine if player is being pushed
            // Maybe move this check to pushedbehaviour?
            float moveDirDiff = Mathf.Abs(Vector2.Angle(attachNormal, rb.velocity));
            Debug.Log("moveDirDiff = " + moveDirDiff);
            // Input to move against block
            if (pushDiff >= 135)
            {
                rb.velocity = Vector2.Scale(direction * speed, scale);
            }
            // Input not to move against block, but block is pushing fast enough
            else if (moveDirDiff <= 60 && rb.velocity.magnitude > ctrlThresh)
            {
                // Switch normal components to get which component "sticks"
                rb.velocity = new Vector2(direction.x * Mathf.Abs(attachNormal.y), direction.y * Mathf.Abs(attachNormal.x)) * speed * pushingMultiplierSide;
                
            }
            // normal movement
            else
            {
                rb.velocity = direction * speed;
            }
        }      
	}

    void OnCollisionEnter2D(Collision2D col) {
        if (!isPushing) {
            // TODO: Check collided object is wall or playblock
            isPushing = true;
            pushed = col.rigidbody;
            attachNormal = col.contacts[0].normal;
            Debug.Log(attachNormal);
            // Figure out whether pushing vertically or horizontally
            // then make scale vector based on that
            if (attachNormal.x == 0f)
            {
                scale.x = pushingMultiplierSide;
                scale.y = pushingMultiplierForward;
            }
            else if (attachNormal.y == 0f)
            {
                scale.x = pushingMultiplierForward;
                scale.y = pushingMultiplierSide;
            }
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
