using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class powerupSpawner : MonoBehaviour {

	public string powerup;
	public GameObject platformSpawnable;
	public float chanceToSpawn;	//Chance to spawn every frame

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		int numPoints = platformSpawnable.transform.childCount;
		int randomPoint = Random.Range (0, numPoints - 1);

		float getChance = Random.Range (0f, 1f);
		if (getChance <= chanceToSpawn) {
			GameObject myBattery = Instantiate (Resources.Load (powerup), 
												platformSpawnable.transform.GetChild(randomPoint).gameObject.transform.position, 
												Quaternion.identity) as GameObject; 
		}
	}
}
