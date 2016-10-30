using UnityEngine;
using System.Collections;

public class timer : MonoBehaviour {

	public float totalTime;
	public float currentTime;

	public bool timeStarted;

	public float minutes;
	public float seconds;

	// Use this for initialization
	void Start () {
		currentTime = totalTime;
		setTime ();
		if (!timeStarted) {
			//Time.timeScale = 0;		//UNCOMMENT WHEN READY
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (timeStarted) {
			currentTime -= Time.deltaTime;
			setTime ();
		}
	}

	void setTime(){
		minutes = Mathf.Floor(currentTime / 60);
		seconds = (Mathf.Floor(currentTime) % 60);
	}

	public string getTimeInString(){
		string timeInString = minutes.ToString ("0") + ":" + seconds.ToString("00");
		return timeInString;
	}

	void startTimer(){
		timeStarted = true;
		Time.timeScale = 1;
	}

	void pauseTimer(){
		timeStarted = false;
		Time.timeScale = 0;
	}
}
