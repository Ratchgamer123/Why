using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTriggerLooper : MonoBehaviour
{
    [SerializeField] private float maxHeight;
    [SerializeField] private float time = 5.0f;
    [SerializeField] private LeanTweenType easeType;

    private void Start()
    {
        LeanTween.moveY(gameObject, maxHeight, time).setLoopPingPong().setEase(easeType);
    }
}
