using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.VFX;

public class UIMenu : MonoBehaviour
{
    public GameObject objectToTween;
    public GameObject fadeoutObject;
    
    public Vector2 inPosition;
    public Vector2 outPosition;
    public int secondsToMove;
    public int secondsToWait;
    public float fadeOutTime;
    public LeanTweenType easeType;
    public TMP_Text songTitle;
    public AudioMixer audioMixer;
    public Slider slider;

    public GameObject mainMenu;
    public GameObject settingsMenu;

    public string menuSongName;

    private void Awake()
    {
        float buffer = PlayerPrefs.GetFloat("masterVolume");
        audioMixer.SetFloat("masterVolume", buffer);
    }

    private void Start()
    {
        StartCoroutine(TweenSongDisplay());
    }

    public void StartGame()
    {
        AudioManager.instance.FakeFadeOut(menuSongName);
        StartCoroutine(FadeIntro());
    }

    private IEnumerator FadeIntro()
    {
        Image image = fadeoutObject.GetComponent<Image>();
        LeanTween.value(gameObject, 0, 1, fadeOutTime).setOnUpdate((float val) =>
        {
            Color c = image.color;
            c.a = val;
            image.color = c;
        });

        yield return new WaitForSeconds(secondsToMove);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void SettingsMenuActivate()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
        float buffer = PlayerPrefs.GetFloat("masterVolume");
        audioMixer.SetFloat("masterVolume", buffer);
        slider.value = buffer;
    }

    public void MainMenuActivate()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator TweenSongDisplay()
    {
        string buffer = AudioManager.instance.Play(menuSongName);
        songTitle.SetText(buffer);
        LeanTween.moveLocal(objectToTween, inPosition, secondsToMove).setEase(easeType).setIgnoreTimeScale(false);
        yield return new WaitForSeconds(secondsToWait);
        LeanTween.moveLocal(objectToTween, outPosition, secondsToMove).setEase(easeType).setIgnoreTimeScale(false);
    }
}
