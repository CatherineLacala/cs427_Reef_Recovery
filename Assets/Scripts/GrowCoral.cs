using System.Collections;
using UnityEngine;

public class GrowCoral : MonoBehaviour 
{
    [Header("Wait Settings")]
    [SerializeField] int targetDeployCount = 3;
    [SerializeField] float delayAfterAllDeployed = 5f;
    [SerializeField] AudioClip milestoneAudioClip;
    [SerializeField] AudioSource milestoneAudioSource;
    
    void Start()
    {
        StartCoroutine(WaitForMilestone());
    }

   IEnumerator WaitForMilestone()
    {
        Debug.Log("Waiting for 3 speakers to be placed AND turned up to 0.5...");
        while (true)
        {
            // first, make sure the player has actually placed all 3 speakers
            if (deploying.activeSpeakers.Count >= targetDeployCount)
            {
                int speakersAtTargetVolume = 0;

                foreach (AudioSource speakerAudio in deploying.activeSpeakers)
                {
                    if (speakerAudio != null && speakerAudio.volume >= 0.5f)
                    {
                        speakersAtTargetVolume++; // Add one to the passing score
                    }
                }
                if (speakersAtTargetVolume >= targetDeployCount)
                {
                    Debug.Log("All 3 speakers reached 0.5 volume! Triggering finale!");
                    break; 
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

        Debug.Log("Doing the final 5-second countdown...");
        yield return new WaitForSeconds(delayAfterAllDeployed);

        if (milestoneAudioClip != null && milestoneAudioSource != null)
        {
            milestoneAudioSource.PlayOneShot(milestoneAudioClip);
        }
    }
}