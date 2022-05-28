using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [SerializeField] private GameObject pauseMenuCanvas;
    [SerializeField] private GameObject menuCanvas;
    [SerializeField] private PlayerMovement player;
    [SerializeField] private AudioMixer audioMixer;

    [SerializeField] private Slider volumeSlider, sensXSlider, sensYSlider;

    private float currentSensX, currentSensY;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        sensXSlider.value = PlayerPrefs.GetFloat("playerSensX");
        sensYSlider.value = PlayerPrefs.GetFloat("playerSensY");
    }

    private void OnEnable()
    {
        PlayerMovement.PauseMenuBindingPressed += CheckMenuState;
    }

    private void OnDisable()
    {
        PlayerMovement.PauseMenuBindingPressed -= CheckMenuState;
    }

    private void CheckMenuState()
    {
        if (gameIsPaused)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetXSens(float value)
    {
        player.sensX = value;
        currentSensX = value;
    }

    public void SetYSens(float value)
    {
        player.sensY = value;
        currentSensY = value;
    }

    public void Resume()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenuCanvas.SetActive(false);
        menuCanvas.SetActive(true);

        Time.timeScale = 1.0f;

        gameIsPaused = false;
        player.enabled = true;

        PlayerPrefs.SetFloat("playerSensX", currentSensX);
        PlayerPrefs.SetFloat("playerSensY", currentSensY);

        audioMixer.GetFloat("masterVolume", out float buffer);
        PlayerPrefs.SetFloat("masterVolume", buffer);
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseMenuCanvas.SetActive(true);
        menuCanvas.SetActive(false);

        Time.timeScale = 0.0f;

        gameIsPaused = true;
        player.enabled = false;
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
    }
}