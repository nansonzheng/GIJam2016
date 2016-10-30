using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

    public void startGame()
    {
        SceneManager.LoadScene(1);
    }
}
