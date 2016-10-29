using UnityEngine;
using System.Collections;

public class PlayerColour : MonoBehaviour
{

    public Color player1;
    public Color player2;
    public Color player3;
    public Color player4;

    Color[] rainbow;

    SpriteRenderer sp;

    public bool chaotic;
    Controls controls;
    int chaos;

    // Use this for initialization
    void Start()
    {
        sp = GetComponent<SpriteRenderer>();
        rainbow = new Color[4] { player1, player2, player3, player4 };
        controls = GetComponent<Controls>();
        if (controls == null)
        {
            Debug.LogWarning("PlayerColour@" + gameObject.name + ": this script is attached to a non-controllable object!");
        }
        switch (controls.playerNum)
        {
            case Controls.Player.Player1:
                chaos = 0;
                break;
            case Controls.Player.Player2:
                chaos = 1;
                break;
            case Controls.Player.Player3:
                chaos = 2;
                break;
            case Controls.Player.Player4:
                chaos = 3;
                break;

        }
        sp.color = rainbow[chaos];
        if (chaotic)
        {
            StartCoroutine(kek());
        }
    }


    IEnumerator kek()
    {
        while (true)
        {
            chaos = (chaos + 1) % 4;
            sp.color = rainbow[chaos];
            yield return null;
        }
    }

}
