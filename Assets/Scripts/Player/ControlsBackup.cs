using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Controls : MonoBehaviour {

    public enum Player {
        Player1,
        Player2,
        Player3,
        Player4
    }

	public float angleErrorThresh;
	public float anglePushingThresh;
	public Vector2 vectorMultiplierSide, vectorMultiplierForward;

	private IList<Collision2D> currentCollisions = new List<Collision2D>();

    public Player playerNum;
    public float speed;
	public float pushingMultiplierSide, pushingMultiplierForward, beingPushedMultiplierForward;
    public bool isPushing;
	public bool beingPushed;
    public float ctrlThresh, crashThresh;
    public Vector2 scale;
    public bool? alive;

    Rigidbody2D rb;
    Rigidbody2D pushed;
    Vector2 attachNormal;

    Vector2 direction;
    float directionF;
    Vector2 vPrev;

    Animator ani;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        scale = Vector2.one;
        ani = GetComponent<Animator>();
        alive = true;
	}
	
	// Update is called once per frame
	void Update () {
		controller ();	//Get Input

        //rb.velocity = direction * speed;
        vPrev = rb.velocity;

		//Checking for modifiers and assign velocities
		if (getBeingPushed ()) {
			handleBeingPushed ();
			rb.velocity = direction * speed;
		} 
		getIsPushing ();

		//Add multipliers and apply our resultant velocity
		if (isPushing || beingPushed) {
			
		}
			

        if (!isPushing)
        {
            rb.velocity = direction * speed;
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

        // Uncomment when ready
        setAnimVars();
    }

    // TODO: LayerMask for different objects
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag(this.tag))
        {
            return;
        }
        if (!isPushing && col.rigidbody != null)
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
                desu();
            }
            else Debug.Log("But nothing happened!");
            // Shrink collider to simulate squish effect?

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

	void controller(){
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
	}

	bool getBeingPushed(){
		beingPushed = (Vector2.Angle (direction.normalized, rb.velocity.normalized) <= angleErrorThresh &&
		currentCollisions.Count >= 1 && rb.velocity >= ctrlThresh);

		return beingPushed;
	}

	void handleBeingPushed(){
		//Assume normal is the velocity we are travelling
		Vector2 normal = rb.velocity.normalized;

		//Here we cancel out movement with the normal direction
		direction = direction.normalized - normal;

		//Modify modifiers
		vectorMultiplierSide = new Vector2 (Mathf.Abs(-normal.y)*pushingMultiplierSide, Mathf.Abs(normal.x)*pushingMultiplierSide);
	}

	bool getIsPushing(){
		if (currentCollisions.Count >= 1 && rb.velocity >= ctrlThresh) {
			foreach (Collision2D c in currentCollisions) {
				if (Vector2.Angle (direction.normalized, c.contacts [0].normal) >= anglePushingThresh) {
					isPushing = true;
					return isPushing;
				}
			}
		}
	}

    // placeholder death anim
    void desu()
    {	/*
        rb.freezeRotation = false;
        rb.angularVelocity = 420.69f;
        StartCoroutine(shrink());*/
		foreach( Transform child in transform )
		{
			child.gameObject.SetActiveRecursively( false );
		} 
		rb.constraints = RigidbodyConstraints2D.FreezePosition;
        GetComponent<Collider2D>().enabled = false;
		GetComponent<SpriteRenderer> ().sortingOrder = 1;
        this.enabled = false;
        Destroy(gameObject, 0.8f);
        alive = false;
		ani.SetTrigger ("death");
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

    Vector2 axesEnabled(Vector2 v) {
        Vector2 ret = Vector2.zero;
        if (v.x != 0)
            ret.x = 1;
        if (v.y != 0)
            ret.y = 1;
        return ret;
    }
}
