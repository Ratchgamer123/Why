using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIMenu : MonoBehaviour
{
    public GameObject objectToTween;
    public Vector2 inPosition;
    public Vector2 outPosition;
    public int secondsToMove;
    public int secondsToWait;
    public LeanTweenType easeType;
    public TMP_Text songTitle;

    private void Start()
    {
        StartCoroutine(TweenSongDisplay());
    }
    public void StartGame()
    {
        AudioManager.instance.FakeFadeOut("GiveEmTheLove");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Settings()
    {
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator TweenSongDisplay()
    {
        songTitle.SetText(AudioManager.instance.Play("GiveEmTheLove"));
        LeanTween.moveLocal(objectToTween, inPosition, secondsToMove).setEase(easeType).setIgnoreTimeScale(false);
        yield return new WaitForSeconds(secondsToWait);
        LeanTween.moveLocal(objectToTween, outPosition, secondsToMove).setEase(easeType).setIgnoreTimeScale(false);
    }
}
