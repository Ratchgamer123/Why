using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.Play("End Song");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoadMenu()
    {
        AudioManager.instance.StopAudio("End Song");
        SceneManager.LoadScene(0);
    }
}
