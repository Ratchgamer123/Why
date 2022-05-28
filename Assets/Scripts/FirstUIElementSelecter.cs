using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FirstUIElementSelecter : MonoBehaviour
{
    [SerializeField] private GameObject uiElement;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(uiElement);
    }
}
