using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class timerUI : MonoBehaviour {

    public Text timerText;
    public GameObject timerScript;

	void Start () {
	
	}
	

	void Update () {
        timerText.text = timerScript.GetComponent<timer>().getTimeInString();
	}
}
