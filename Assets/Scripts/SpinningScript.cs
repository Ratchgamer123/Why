using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningScript : MonoBehaviour
{
    void Start()
    {
        LeanTween.rotateAround(gameObject, Vector3.forward, 360, 2.5f).setLoopClamp();
    }
}
