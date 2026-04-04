using UnityEngine;

public class BoatTrigger : MonoBehaviour
{
    public BackgroundAudio audioManager;
    public AudioClip boatNoise;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (audioManager != null && audioManager.oneShotSource != null)
            {
                audioManager.oneShotSource.PlayOneShot(boatNoise);
            }
        }
    }
}