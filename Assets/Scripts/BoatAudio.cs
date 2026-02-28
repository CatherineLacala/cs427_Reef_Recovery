using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatAudio : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;

    public AudioClip boatIntroClip;
    public AudioClip solarPanelClip;
    public AudioClip reefClip;

    private bool boatAudioPlayed = false;
    
    void Start()
    {
        StartCoroutine(PlayIntroAfterDelay());
    }
    IEnumerator PlayIntroAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        audioSource.PlayOneShot(boatIntroClip);
    }

    public void PlaySolarPanelAudio()
    {
        if (!boatAudioPlayed)
        {
            audioSource.PlayOneShot(solarPanelClip);
            boatAudioPlayed = true;
        }
    }

    public void PlayReefAudio()
    {
        audioSource.PlayOneShot(reefClip);
    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
}
