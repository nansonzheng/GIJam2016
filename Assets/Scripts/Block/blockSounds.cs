using UnityEngine;
using System.Collections;

public class blockSounds : MonoBehaviour {

    public GameObject mainCamera;
    public AudioSource source;
    public AudioClip bumpSound;

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Wall") || col.gameObject.CompareTag("Block"))
        {
            source.PlayOneShot(bumpSound, 1f);
            mainCamera.GetComponent<Camera_Shake>().shakeCamera(0.3f, 0.3f);
        }
    }
}
