using UnityEngine;
using System.Collections;

public class cameraBehaviour : MonoBehaviour {

	public float colliderMargin = 2f; //This is collider offset from camera view

	private Camera camera;
	private Vector2 cameraPos;
	private Vector2 cameraSize;

	private BoxCollider2D limitColliderTop;	//Outer top collider (set to camera view size)
	private BoxCollider2D limitColliderRight;	//Outer right collider (set to camera view size)
	private BoxCollider2D limitColliderBottom;	//Outer Bottom collider (set to camera view size)
	private BoxCollider2D limitColliderLeft;	//Outer Left collider (set to camera view size)

	private GameObject margin;	//Margin for camera moving

	// Use this for initialization
	void Start () {
		//Get camera size and position
		camera = GetComponent<Camera>();
		cameraPos = transform.position;
		cameraSize.x = Vector2.Distance (camera.ScreenToWorldPoint(new Vector2(0,0)),camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
		cameraSize.y = Vector2.Distance (camera.ScreenToWorldPoint(new Vector2(0,0)),camera.ScreenToWorldPoint(new Vector2(0, Screen.height))); 

		//Init colliders
		limitColliderTop = gameObject.AddComponent<BoxCollider2D>();
		limitColliderRight = gameObject.AddComponent<BoxCollider2D>();
		limitColliderBottom = gameObject.AddComponent<BoxCollider2D>();
		limitColliderLeft = gameObject.AddComponent<BoxCollider2D>();
		 
		margin = new GameObject("margin");
		margin.transform.parent = gameObject.transform;  
		//margin = gameObject.AddComponent<BoxCollider2D>();

		//Set the collider positions
		limitColliderTop.offset = new Vector2(0, cameraSize.y * 0.5f+ colliderMargin * 0.5f) + cameraPos;
		limitColliderTop.size = new Vector2 (cameraSize.x, colliderMargin);

		limitColliderRight.offset = new Vector2(cameraSize.x * 0.5f +  colliderMargin * 0.5f, 0) + cameraPos;
		limitColliderRight.size = new Vector2 (colliderMargin, cameraSize.y);

		limitColliderBottom.offset = new Vector2(0, -1 * cameraSize.y * 0.5f - colliderMargin * 0.5f) + cameraPos;
		limitColliderBottom.size = new Vector2 (cameraSize.x, colliderMargin);

		limitColliderLeft.offset = new Vector2(-1 * cameraSize.x * 0.5f - colliderMargin * 0.5f, 0) + cameraPos;
		limitColliderLeft.size = new Vector2 (colliderMargin, cameraSize.y);

		//margin.size = new Vector2 (cameraSize.x * 0.5f, cameraSize.y * 0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
