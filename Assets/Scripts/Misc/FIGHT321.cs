using UnityEngine;
using System.Collections;

public class FIGHT321 : MonoBehaviour {

    private bool isCoroutineExecuting = false;
    public GameObject timerObject;
    public GameObject panel;

    public float delay = 0f;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(ExecuteAfterTime(this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length + delay));
    }


    IEnumerator ExecuteAfterTime(float time)
    {
        if (isCoroutineExecuting)
            yield break;

        isCoroutineExecuting = true;

        yield return new WaitForSeconds(time);

        timerObject.GetComponent<timer>().startTimer();
        panel.SetActive(false);
        panel.SetActive(false);

        isCoroutineExecuting = false;
    }

}
