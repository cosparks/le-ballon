using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMusic : MonoBehaviour
{
    private AudioSource audioSource;

    private float startVolume;
    private float slope;
    private float timer;
    private bool startFade;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (startFade)
        {
            FadeOutMusic();
        }
    }

    private void FadeOutMusic()
    {
        timer += Time.deltaTime;
        audioSource.volume = slope * timer + startVolume;
    }

    public void StartFade(float fadeTime)
    {
        startVolume = audioSource.volume;
        slope = startVolume / (-fadeTime);
        startFade = true;
    }
}
