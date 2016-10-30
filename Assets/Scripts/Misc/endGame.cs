using UnityEngine;
using System.Collections;

public class endGame : MonoBehaviour {

    public GameObject timer;
    public GameObject end;

	void Update () {
        if (timer.GetComponent<timer>().currentTime <= 0)
        {
            timer.GetComponent<timer>().pauseTimer();
            gameObject.SetActive(true);
        }
	}
}

