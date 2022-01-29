using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLerper : MonoBehaviour
{
    public Transform start;
    public Transform end;

    public float duration;

    public GameObject objectToLerp;
    public bool shouldBeInstantiated;

    private void OnTriggerEnter(Collider other)
    {
        if (shouldBeInstantiated)
        {
            Instantiate(objectToLerp, start);
            Lerp();
        }
        else
        {
            Lerp();
        }
    }

    private void Lerp()
    {
        LeanTween.move(objectToLerp, end, duration);
    }
}
