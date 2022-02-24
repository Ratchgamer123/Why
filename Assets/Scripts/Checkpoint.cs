using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour, IInteractable
{
    Animator animator;
    AudioSource audioSource;
    public Transform respawnPoint;

    public static event Action<Transform> SetCurrentCheckpoint;
    public static event Action ShowNotification;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Interact()
    {
        animator.Play("ButtonPress");
        audioSource.PlayOneShot(audioSource.clip);
        SetCurrentCheckpoint?.Invoke(respawnPoint);
        ShowNotification?.Invoke();
    }
}
