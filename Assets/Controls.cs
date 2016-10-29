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
    Rigidbody2D rb;

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
	}
}
