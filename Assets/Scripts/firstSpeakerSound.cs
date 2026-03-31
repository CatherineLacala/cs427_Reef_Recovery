using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class firstSpeakerSound : CAVE2Interactable
{
    private AudioSource audioSource;
    public AudioClip loopClip;
    private bool isLooping = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();

        audioSource.clip = loopClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 1.0f;
    }

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == CAVE2.Button.Button3)
        {
            if (!isLooping)
            {
                audioSource.Play();
                isLooping = true;
            }
        }
    }
}
