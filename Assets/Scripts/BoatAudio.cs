using System.Collections;
using UnityEngine;

public class BoatAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public AudioClip boatIntroClip;
    public AudioClip solarPanelClip;
    public AudioClip reefClip;

    public AudioClip walkieTalkieClip;
    public AudioClip overWalkieClip;

    private bool boatAudioPlayed = false;
    private bool isPlaying = false;

    void Start()
    {
        StartCoroutine(PlayIntroAfterDelay());
    }

    IEnumerator PlayIntroAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(PlayWithWalkie(boatIntroClip));
    }

    public void PlaySolarPanelAudio()
    {
        if (!boatAudioPlayed && !isPlaying)
        {
            boatAudioPlayed = true;
            StartCoroutine(PlayWithWalkie(solarPanelClip));
        }
    }

    public void PlayReefAudio()
    {
        if (!isPlaying)
        {
            StartCoroutine(PlayWithWalkie(reefClip));
        }
    }

    IEnumerator PlayWithWalkie(AudioClip mainClip)
    {
        isPlaying = true;

        // Play walkie start sound
        audioSource.PlayOneShot(walkieTalkieClip);
        yield return new WaitForSeconds(walkieTalkieClip.length);

        // Play main dialogue
        audioSource.PlayOneShot(mainClip);
        yield return new WaitForSeconds(mainClip.length);

        // Play walkie end sound
        audioSource.PlayOneShot(overWalkieClip);
        yield return new WaitForSeconds(overWalkieClip.length);

        isPlaying = false;
    }
}