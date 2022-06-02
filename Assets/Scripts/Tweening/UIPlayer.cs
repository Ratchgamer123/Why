using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPlayer : MonoBehaviour
{
    [SerializeField] private GameObject checkpointNotification;
    [SerializeField] private GameObject levelNotification;
    [SerializeField] private GameObject fadeOutObject;
    [SerializeField] private float fadeOutTime;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float checkpointDestinationY;
    [SerializeField] private float checkpointOriginY;
    [SerializeField] private float levelDestinationY;
    [SerializeField] private float levelNotifOriginY;
    [SerializeField] private float checkpointLerpTime = 1.0f;
    [SerializeField] private float levelLerpTime = 1.0f;
    [SerializeField] private LeanTweenType checkpointLerpType;
    [SerializeField] private LeanTweenType levelLerpType;
    [SerializeField] private TMP_Text levelNotifText;
    public bool shouldFade;

    private bool tweenComplete = true;

    private void Start()
    {
        levelNotifText.text = SceneManager.GetActiveScene().name;
        StartCoroutine(ShowLevelNotification());
    }

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

    private IEnumerator ShowLevelNotification()
    {
        if (shouldFade)
        {
            Image image = fadeOutObject.GetComponent<Image>();
            image.color = Color.black;
            LeanTween.value(gameObject, 1, 0, fadeOutTime).setOnUpdate((float val) =>
            {
                Color c = image.color;
                c.a = val;
                image.color = c;
            });
        }
        LeanTween.moveLocal(levelNotification, new Vector2(0.0f, levelDestinationY), levelLerpTime).setEase(levelLerpType).setIgnoreTimeScale(false);
        yield return new WaitForSeconds(2.0f);
        LeanTween.moveLocal(levelNotification, new Vector2(0.0f, levelNotifOriginY), levelLerpTime).setEase(levelLerpType).setIgnoreTimeScale(false);
    }
}
