using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private GameObject checkpointNotification;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float checkpointDestinationY;
    [SerializeField] private float checkpointOriginY;
    [SerializeField] private float checkpointLerpTime = 1.0f;
    [SerializeField] private LeanTweenType checkpointLerpType;

    private bool tweenComplete = true;

    private void OnEnable()
    {
        Checkpoint.ShowNotification += CheckpointNotification;
    }

    private void OnDisable()
    {
        Checkpoint.ShowNotification -= CheckpointNotification;
    }

    public void CheckpointNotification()
    {
        if (!tweenComplete) { return; }
        StartCoroutine(CheckpointDelay());
    }

    private IEnumerator CheckpointDelay()
    {
        tweenComplete = false;
        LeanTween.moveLocal(checkpointNotification, new Vector2(0.0f, checkpointDestinationY), checkpointLerpTime).setEase(checkpointLerpType).setIgnoreTimeScale(false);
        yield return new WaitForSeconds(1.5f);
        LeanTween.moveLocal(checkpointNotification, new Vector2(0.0f, checkpointOriginY), checkpointLerpTime).setEase(checkpointLerpType).setIgnoreTimeScale(false).setOnComplete(() => tweenComplete = true);
    }
}
