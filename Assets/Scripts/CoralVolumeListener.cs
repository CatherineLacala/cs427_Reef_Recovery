using UnityEngine;

public class CoralVolumeListener : MonoBehaviour
{
   
    public SpeakerReefTrigger linkedReef; 
    
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // make sure we actually have audio and a linked reef to check
        if (linkedReef != null && audioSource != null)
        {
            // if the user clicks the Up arrow enough times to hit 0.5 (50%)
            if (audioSource.volume >= 0.5f)
            {
                // trigger the coral
                linkedReef.OnSpeakerPlaced();
                this.enabled = false; 
            }
        }
    }
}