using UnityEngine;
using System;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    public AudioMixer audioMixer;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;

            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.outputAudioMixerGroup = sound.audioMixerGroup;
        }
    }

    public string Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} wurde nicht gefunden!");
        }
        s.source.Play();
        return s.source.clip.name;
    }

    public void StopAudio(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} konnte nicht gestoppt werden!");
        }
        s.source.Stop();
    }

    public void FakeFadeOut(string name)
    {
        StartCoroutine(FadeOut(name));
    }

    public IEnumerator FadeOut(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound {name} konnte nicht gefunden werden!");
        }

        float buffer = s.source.volume;

        while (s.source.volume > 0.001f)
        {
            s.source.volume = Mathf.Lerp(s.source.volume, 0.0f, s.fadeOutSpeed * Time.deltaTime);
            yield return null;
        }

        s.source.Stop();
        s.source.volume = buffer;
    }
}

