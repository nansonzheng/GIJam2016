using UnityEngine;
using System.Collections;

public class spawnPoint : MonoBehaviour {

	public Controls.Player playerNum;
	private GameObject refToPlayer;

	// Use this for initialization
	void Start () {
		//spawnPlayer ();
	}
	
	// Update is called once per frame
	void Update () {
		refToPlayer = GameObject.Find ("Player"+playerNum);
	}
}
