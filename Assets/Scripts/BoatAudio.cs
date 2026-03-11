using System.Collections;
using System.Numerics;
using UnityEngine;

public class BoatAudio : MonoBehaviour
{
    public Transform playerTransform;
    public AudioSource audioSource;

    public AudioClip boatIntroClip;
    public AudioClip solarPanelClip;
    public AudioClip reefClip;
    public AudioClip deepIntoWaterClip;
    public AudioClip deepOntoReefClip;

    public AudioClip walkieTalkieClip;
    public AudioClip overWalkieClip;

    public float depthThreshold1 = 100f;
    public float depthThreshold2 = 20f;

    private bool boatAudioPlayed = false;
    private bool deepIntoWaterAudioPlayed = false;
    private bool deepOntoReefAudioPlayed = false;
    private bool isPlaying = false;

    void Start()
    {
        StartCoroutine(PlayIntroAfterDelay());
    }

    void Update()
    {
        PlayDeepIntoWaterAudio();
        PlayDeepOntoReefAudio();
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

    public void PlayDeepIntoWaterAudio()
    {
        if (!deepIntoWaterAudioPlayed && (playerTransform.transform.position.y < depthThreshold1) && !isPlaying)
        {
            //StartCoroutine(PlayWithWalkie(deepIntoWaterClip));
            print("debug: audio played");
            deepIntoWaterAudioPlayed = true;
        }
    }

    public void PlayDeepOntoReefAudio()
    {
        if (!deepOntoReefAudioPlayed && (playerTransform.transform.position.y < depthThreshold1) && !isPlaying)
        {
            //StartCoroutine(PlayWithWalkie(deepOntoReefClip));
            print("debug: audio played");
            deepOntoReefAudioPlayed = true;
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