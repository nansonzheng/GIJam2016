using UnityEngine;
using System.Collections;

public class MusicScript : MonoBehaviour {

    public GameObject music;

    void Awake()
    {
        DontDestroyOnLoad(music);
    }
}
