using UnityEngine;
using System.Collections;

public class PowerPickUp : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D receiver)
    {
        Debug.Log("Pickup Interacted with " + receiver.gameObject.name);
        if (receiver.gameObject.CompareTag("Player"))
        {
            receiver.gameObject.SendMessage("IncrementPower");
        }
        Destroy(gameObject);
    }
}
