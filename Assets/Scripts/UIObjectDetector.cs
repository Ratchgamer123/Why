using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIObjectDetector : MonoBehaviour
{
    public static UIObjectDetector instance;
    public EventSystem es;
    public PointerEventData ped;
    public List<RaycastResult> rr;
    public bool debug;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        ped = new PointerEventData(null);
        rr = new List<RaycastResult>();
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) && debug)
        {
            IsOverUIObject();
        }
    }

    public bool IsOverUIObject()
    {
        ped.position = Input.mousePosition;
        rr.Clear();

        es.RaycastAll(ped, rr);
        if (rr.Count != 0)
        {
            Debug.Log("Object Clicked on " + rr[0].gameObject.name + " Parent is " + rr[0].gameObject.transform.parent.name);
            if (rr[0].gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
}