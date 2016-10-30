using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {

    public enum Player {
        Player1 = 1,
        Player2 = 2,
        Player3 = 3,
        Player4 = 4
    }

    public Player playerNum;
    public float speed;
    public float pushingMultiplierSide, pushingMultiplierForward;
    public bool isPushing;
    public bool beingPushed;
    public float ctrlThresh, crashThresh;
    Vector2 scale;
    public bool? alive;

    Rigidbody2D rb;
    Rigidbody2D pushed;
    Vector2 attachNormal;

    Vector2 direction;
    float directionF;
    Vector2 vPrev;

    Animator ani;
    GameObject EX;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        scale = Vector2.one;
        ani = GetComponent<Animator>();
        EX = transform.Find("EX").gameObject;
        alive = true;
        // Spawn facing down pls
        directionF = 4;

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
            case Player.Player4:
                direction.x = Input.GetAxis("Horizontal4");
                direction.y = Input.GetAxis("Vertical4");
                break;
        }
        if (direction != Vector2.zero) {
            directionF = Vector2.Angle(Vector2.up, direction);
        // If going left, need to add 180 to get actual angle
        if (direction.x < 0)
        {
            directionF = 360 - directionF;
        }
        // Divide to get number in range of 0 to 8, mod to get nearest int
        directionF = (Mathf.Round(directionF / 45)) % 8;
        }
        
            
        //rb.velocity = direction * speed;
        vPrev = rb.velocity;
        if (!isPushing)
        {
            rb.velocity = direction * speed;
            beingPushed = false;
        }
        else
        {
            // Determine if movement is to push block
            float pushDiff = Vector2.Angle(attachNormal, direction);
            // Determine if player is being pushed
            // Maybe move this check to pushedbehaviour?
            float moveDirDiff = Vector2.Angle(attachNormal, rb.velocity);

            //Debug.Log("pushDiff " + pushDiff + ", moveDirDiff " + moveDirDiff);
            // Input to move against block
            if (pushDiff >= 135)
            {
                rb.velocity = Vector2.Scale(direction * speed, scale);
                beingPushed = false;
            }
            // Input not to move against block, but block is pushing fast enough
            else if (moveDirDiff <= 60 && rb.velocity.magnitude > ctrlThresh)
            {
                beingPushed = true;
                // Switch normal components to get which component "sticks"
                rb.velocity = new Vector2(direction.x * Mathf.Abs(attachNormal.y), direction.y * Mathf.Abs(attachNormal.x)) * speed * pushingMultiplierSide;
                
            }
            // normal movement
            else
            {
                beingPushed = false;
                rb.velocity = direction * speed;
            }
        }

        // Uncomment when ready
        enableEX();
        setAnimVars();
    }


    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag(this.tag))
        {
            return;
        }
        if (!isPushing)
        {
            // Pushable objects have rigidbody
            if (col.rigidbody != null)
            {
               isPushing = true;
               pushed = col.rigidbody;
               attachNormal = col.contacts[0].normal;
               Debug.Log("attachnormal " + attachNormal);
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
            // else wall?
           else
            {
                // change isPushing to true to enable collisions
                // pushed remains null to indicate wall
                // at collisionexit, col.rigidbody is null == pushed.
                isPushing = true;
                attachNormal = col.contacts[0].normal;
                scale = Vector2.one;
                Debug.Log("attack on titan");
            }
            
        }
        // Else, probably being squished
        else
        {
            Vector2 secondNormal = col.contacts[0].normal;

            if (Vector2.Scale(attachNormal, secondNormal) == Vector2.zero)
            {
                Debug.Log("But it failed! " + attachNormal + " " + secondNormal );
                return;
            }
            Debug.Log(Vector2.Scale(attachNormal, secondNormal));
            // If vectors cancel out, then they're opposite
            Vector2 vDiff = - Vector2.Scale(vPrev, attachNormal);
            if (col.rigidbody != null)
            {
                vDiff += Vector2.Scale(col.rigidbody.velocity, secondNormal);
            }
            // Else the thing isn't meant to move.
            if (vDiff.magnitude > crashThresh)
            {
                Debug.Log("Squished with " + col.gameObject + ", spd: " + vDiff.magnitude);
                death();
            }
            else Debug.Log("But nothing happened!");

        }
    }


    void OnCollisionExit2D(Collision2D col) {
        if (isPushing && col.rigidbody == pushed)
        {
            isPushing = false;
            pushed = null;
            attachNormal = Vector2.zero;
        }

    }

    // placeholder death anim
    void death()
    {
        GetComponent<Collider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        this.enabled = false;
        Destroy(gameObject, 0.7f);
        alive = false;
        ani.SetTrigger("death");
        GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    void setAnimVars()
    {
        ani.SetFloat("movSpeed", rb.velocity.magnitude);
        ani.SetFloat("direction", directionF);
    }

    IEnumerator shrink()
    {
        for (float i = 0; i < 1; i += 0.01f)
        {
            transform.localScale= new Vector3(1 - i, 1-i, 1);
            yield return null;
        }
    }

    // Returns Vector2 with each component = 1 where not equal to 0
    Vector2 axesEnabled(Vector2 v) {
        Vector2 ret = Vector2.zero;
        if (v.x != 0)
            ret.x = 1;
        if (v.y != 0)
            ret.y = 1;
        return ret;
    }

    void enableEX()
    {
        EX.SetActive(beingPushed);
    }
}
