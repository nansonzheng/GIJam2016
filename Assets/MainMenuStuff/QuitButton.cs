using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour {

    public void quitGame()
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
