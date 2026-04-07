using UnityEngine;

public class VolumeButton : CAVE2Interactable
{
    [Header("Volume Settings")]
    [Tooltip("Positive number to increase volume (e.g., 0.1), negative to decrease (e.g., -0.1)")]
    public float volumeChangeAmount = 0.1f;
    
    [SerializeField] CAVE2.Button clickButton = CAVE2.Button.Button3;

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == clickButton)
        {
            AudioSource speakerAudio = GetComponentInParent<AudioSource>();

            if (speakerAudio != null)
            {
                float newVolume = speakerAudio.volume + volumeChangeAmount;
                speakerAudio.volume = Mathf.Clamp(newVolume, 0f, 1f);
                
                
                if (volumeChangeAmount > 0)
                {
                    Debug.Log("Volume went UP! Current volume is now: " + speakerAudio.volume);
                }
                else if (volumeChangeAmount < 0)
                {
                    Debug.Log("Volume went DOWN! Current volume is now: " + speakerAudio.volume);
                }
             
            }
            else
            {
                Debug.LogWarning("VolumeButton clicked, but could not find an AudioSource on the parent object!");
            }
        }
    }
}