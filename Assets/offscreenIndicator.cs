using UnityEngine;
using System.Collections;

public class linearFunction{
	public float slope;
	public float b;

	public bool infiniteSlope = false;

	//Constructors
	public linearFunction(Vector2 p1, Vector2 p2){
		if (p2.x == p1.x) {
			infiniteSlope = true;
			slope = 0;
			b = p1.x;

		} 
		else {
			slope = (p2.y - p1.y) / (p2.x - p1.x);
			b = p1.y - (p1.x * slope);
		}
	}

	public linearFunction(float slope, float b, bool infiniteSlope){
		if (infiniteSlope) {
			this.infiniteSlope = true;
			this.slope = 0;
			this.b = b;
		} else {
			this.infiniteSlope = false;
			this.slope = slope;
			this.b = b;
		}
	}

	public float getY(float x){
		if (infiniteSlope)
			return 0;
		else
			return slope * x + b;
	}

	public Vector2? intersectswithMe(linearFunction f){
		Vector2 poi;

		if (infiniteSlope) {
			poi.x = b;
			poi.y = f.slope * b + f.b; 
		} else if (f.infiniteSlope) {
			poi.x = f.b;
			poi.y = slope * f.b + b;
		} else if (f.slope == this.slope) {
			return null;	//If No intersection (or too many), give it nothing
		} 
		else {
			poi.x = (f.b - this.b) / (this.slope - f.slope);
			poi.y = slope * poi.x + b;
		}

		Vector2? return_poi = poi;

		return return_poi;
	}
}

public class offscreenIndicator : MonoBehaviour {

	public float offset_from_screen;
	public int playerNum;

	private GameObject indicator;
	private Vector2 cameraPos;
	private Vector2 cameraSize;
	private linearFunction lf_indicator;

	// Use this for initialization
	void Start () {
		string path_to_indic = "Sprites/Misc/arrow";
		indicator = Instantiate(Resources.Load("Prefabs/Misc/indic")) as GameObject;
		Sprite[] indics = Resources.LoadAll<Sprite> (path_to_indic);
		indicator.GetComponent<SpriteRenderer> ().sprite = indics[playerNum - 1];

		cameraPos = Camera.main.transform.position;
		cameraSize.x = Vector2.Distance (Camera.main.ScreenToWorldPoint(new Vector2(0,0)),Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
		cameraSize.y = Vector2.Distance (Camera.main.ScreenToWorldPoint(new Vector2(0,0)),Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height))); 
	}
	
	// Update is called once per frame
	void Update () {
		if (GetComponent<Renderer> ().isVisible) {
			indicator.SetActive (false);
			indicator.GetComponent<Renderer>().enabled = false;
		} 
		else {
			cameraPos = Camera.main.transform.position; //Set the camera position

			lf_indicator = new linearFunction(cameraPos, transform.position); //Draw the line
			//Draw our screen lines
			linearFunction left_screen = new linearFunction(0, -1 * cameraSize.x * 0.5f + cameraPos.x, true);
			linearFunction right_screen = new linearFunction(0, cameraSize.x * 0.5f + cameraPos.x, true);
			linearFunction top_screen = new linearFunction(0, cameraSize.y * 0.5f + cameraPos.y, false);
			linearFunction bottom_screen = new linearFunction(0, -1* cameraSize.y * 0.5f + cameraPos.y, false);

			rotateIndicator(AngleBetweenVector2(new Vector2(0,1), ((Vector2)transform.position - cameraPos)));

			//Check our intersection with all lines (if line appears in the renderer & is between our position and cameraCenter, then we found it)
			Vector2? poi = lf_indicator.intersectswithMe(left_screen);
			Debug.Log (poi.Value);
			if (ifIsValidIndicatorLocation (poi)) {
				left_screen = new linearFunction(0, -1 * cameraSize.x * 0.5f + cameraPos.x + offset_from_screen*2, true);
				poi = lf_indicator.intersectswithMe(left_screen);
				indicator.transform.position = poi.Value;
			}

			poi = lf_indicator.intersectswithMe(right_screen);
			if (ifIsValidIndicatorLocation (poi)) {
				right_screen = new linearFunction(0, cameraSize.x * 0.5f + cameraPos.x - offset_from_screen*2, true);
				poi = lf_indicator.intersectswithMe(right_screen);
				indicator.transform.position = poi.Value;
			}

			poi = lf_indicator.intersectswithMe(top_screen);
			if (ifIsValidIndicatorLocation (poi)) {
				top_screen = new linearFunction(0, cameraSize.y * 0.5f + cameraPos.y - offset_from_screen, false);
				poi = lf_indicator.intersectswithMe(top_screen);
				indicator.transform.position = poi.Value;
			}

			poi = lf_indicator.intersectswithMe(bottom_screen);
			if (ifIsValidIndicatorLocation (poi)) {
				bottom_screen = new linearFunction(0, -1* cameraSize.y * 0.5f + cameraPos.y + offset_from_screen, false);
				poi = lf_indicator.intersectswithMe(bottom_screen);
				indicator.transform.position = poi.Value;
			}
				
			indicator.SetActive (true);
			indicator.GetComponent<Renderer>().enabled = true;
		}
	}

	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		float xdifference = vec1.x - vec2.x;
		if (xdifference >= 0) {
			return Vector2.Angle (vec1, vec2);
		}
		else {
			return 360 - Vector2.Angle (vec1, vec2);
		}
	}

	private void rotateIndicator(float angle){
		indicator.transform.eulerAngles = new Vector3(0,0,angle);
	}

	private bool betweenFloats(float limit1, float limit2, float between){
		if ((limit1 <= between && between <= limit2) || (limit2 <= between && between <= limit1))
			return true;
		else
			return false;
	}

	private bool ifIsValidIndicatorLocation(Vector2? poi){
		if (poi != null) {
			if (cameraPos.x - cameraSize.x * 0.5f <= poi.Value.x && poi.Value.x <= cameraPos.x + cameraSize.x * 0.5f
				&& cameraPos.y - cameraSize.y * 0.5f <= poi.Value.y && poi.Value.y <= cameraPos.y + cameraSize.y * 0.5f) {
				if (betweenFloats (transform.position.x, cameraPos.x, poi.Value.x) && betweenFloats (transform.position.y, cameraPos.y, poi.Value.y)) {
					return true;
				}
			}
		}

		return false;
	} 
}
