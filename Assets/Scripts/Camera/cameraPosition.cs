using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class cameraPosition : MonoBehaviour {


    Camera cam;
    HashSet<GameObject> playerPos;
    Vector2 centeredPos;

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        playerPos = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 temp = new Vector2();
	    foreach(GameObject p in playerPos)
        {
            bool? pStat = p.GetComponent<Controls>().alive;
            if (!pStat.HasValue || !pStat.Value)
            {
                playerPos.Remove(p);
                continue;
            }
            else
            {
                temp.x += p.transform.position.x;
                temp.y += p.transform.position.y;
            }
        }
        centeredPos = temp / playerPos.Count;
        cam.transform.position = centeredPos;
	}
    
    void KillPlayer(GameObject player)
    {
        playerPos.Remove(player);
    }
}
