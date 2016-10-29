using UnityEngine;
using System.Collections;

public class Camera_Shake : MonoBehaviour
{
    public Transform camTransform;

    public float shakeDuration = 0f;

    public float shakeAmount = 0.3f;
    public float decreaseFactor = 1.0f;

    Vector3 originalPos;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    public void shakeCamera(float duration, float intensity)
    {
        shakeAmount = intensity;
        shakeDuration = duration;
    }

    void Update()
    {
        originalPos = camTransform.localPosition;
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
        }
        else
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
        }
    }
}