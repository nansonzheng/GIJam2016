using UnityEngine;
using System.Collections;

public class spawnPoint : MonoBehaviour {

	public Controls.Player playerNum;
	public float totalDeathTimer;
	public float curDeathTimer;

	private GameObject refToPlayer;

	// Use this for initialization
	void Start () {
		spawnIfDeadOrDeathTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if(refToPlayer == null)
			spawnIfDeadOrDeathTimer();
	}


	void spawnIfDeadOrDeathTimer(){
		if (curDeathTimer <= 0) {	//spawn player because penalty is over
			curDeathTimer = totalDeathTimer;
			refToPlayer = Instantiate (Resources.Load ("Prefabs/Player/player")) as GameObject;

			//Set some stuff
			refToPlayer.transform.position = transform.position;
			refToPlayer.gameObject.GetComponent<Controls> ().playerNum = playerNum;
			refToPlayer.gameObject.GetComponent<offscreenIndicator> ().playerNum = (int)playerNum;
		} 
		else {
			curDeathTimer -= Time.deltaTime;	//Reduce penalty
		}
	}
}
