using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMover : MonoBehaviour
{
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject go;
    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.CompareTag("Ball"))
        {
            LeanTween.move(go, destination, 2.0f).setIgnoreTimeScale(false).setEase(LeanTweenType.easeInOutBack);
        }
    }
}
