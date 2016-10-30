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
    public bool? alive;
    public int powerIncrements, powerMax;

    Rigidbody2D rb;
    Rigidbody2D pushed;
    Vector2 attachNormal;
    Vector2 baseScale, scale;
    bool normalIsX;

    Vector2 direction;
    float directionF;
    Vector2 vPrev;

    Animator ani;
    Animator EX;

    IList<GameObject> touching;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        baseScale = Vector2.one;
        ani = GetComponent<Animator>();
        EX = transform.Find("EX").GetComponent<Animator>();
        alive = true;
        // Spawn facing down pls
        directionF = 4;

        StartCoroutine(EXenable());
        touching = new List<GameObject>();
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
        
        setAnimVars();
    }


    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.CompareTag(this.tag) || col.gameObject.CompareTag("PowerUp"))
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
                // Figure out whether pushing vertically or horizontally
                // then make scale vector based on that
                if (attachNormal.x == 0f)
                {
                    baseScale.x = pushingMultiplierSide;
                    baseScale.y = pushingMultiplierForward;
                }
                else if (attachNormal.y == 0f)
                {
                    baseScale.x = pushingMultiplierForward;
                    baseScale.y = pushingMultiplierSide;
                }
                UpdateScale();
            }
            // else wall?
           else
            {
                // change isPushing to true to enable collisions
                // pushed remains null to indicate wall
                // at collisionexit, col.rigidbody is null == pushed.
                isPushing = true;
                attachNormal = col.contacts[0].normal;
                baseScale = Vector2.one;
                UpdateScale();
                Debug.Log("attack on titan");
            }
            
        }
        // Else, probably being squished
        else
        {
            Vector2 vectorOfAttack = transform.position - col.transform.position;
            Vector2 secondNormal = col.contacts[0].normal;

            Vector2 vTotal = rb.velocity;
            if (pushed)
            {
                vTotal += pushed.velocity;
            }
            if (col.rigidbody)
            {
                vTotal -= col.rigidbody.velocity;
            }
            
            float angleOfAttack = Vector2.Angle(secondNormal, vTotal);

            Debug.Log("v=" + vTotal + ", angle=" + angleOfAttack);
            if (pushed && col.rigidbody)
            {
                float a = Vector2.Angle(transform.position - pushed.transform.position, transform.position - col.gameObject.transform.position);
                if (a < 90)
                {
                    Debug.Log("you shouldn't be dying here");
                    return;
                }
            }

            if ((rb.velocity == Vector2.zero || angleOfAttack >= 120) && vTotal.magnitude > crashThresh)
            {
                death();
            }

        }
        if (!touching.Contains(col.gameObject))
        {
            touching.Add(col.gameObject);
        }

    }


    void OnCollisionExit2D(Collision2D col) {
        if (!touching.Remove(col.gameObject))
        {
            Debug.LogWarning("OnCollisionExit2D: Tried to remove a thing not in the list!");
        }
        if (isPushing && col.rigidbody == pushed)
        {
            // Initial set to null
            pushed = null;
            //TODO if something else in list
            if (touching.Count != 0)
            {
                foreach (GameObject c in touching)
                {
                    pushed = c.GetComponent<Rigidbody2D>();
                    // It's possible a wall/immovable obj is added
                    // Prioritize on moveables
                    if (pushed != null) {
                        break;
                    }
                }
            }
            // If nothing in list
            else
            {
                isPushing = false;
                attachNormal = Vector2.zero;
            }
        }
    }
    

    // placeholder death anim
    void death()
    {
        GetComponent<Collider2D>().enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
        this.enabled = false;
        alive = false;
        ani.SetTrigger("death");
        GetComponent<SpriteRenderer>().sortingOrder = 1;
        Destroy(gameObject, 1.5f);
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
        EX.gameObject.SetActive(beingPushed);
        EX.SetBool("beingPushed", beingPushed);
    }

    IEnumerator EXenable()
    {
        bool recorded = !beingPushed;
        while (true)
        {
            if (recorded != beingPushed)
            {
                EX.SetBool("beingPushed", beingPushed);
                recorded = beingPushed;
            }
            yield return null;
        }
    }

    void SetPowerIncrements(int power)
    {
        if (power > powerMax)
        {
            Debug.Log(gameObject+" already has max power!");
            return;
        }
        if (power < 0)
        {
            powerIncrements = 0;
            return;
        }

        powerIncrements = power;
        UpdateScale();
    }

    void IncrementPower()
    {
        if (powerIncrements < powerMax)
        {
            SetPowerIncrements(powerIncrements + 1);
        }
    }

    void UpdateScale()
    {
        if (baseScale != Vector2.one)
        {
            scale = baseScale + (axesEnabled(attachNormal) * powerIncrements);
            Debug.Log(scale + " " + axesEnabled(attachNormal) * powerIncrements);
        }
        else
        {
            scale = baseScale;
        }
    }
}

