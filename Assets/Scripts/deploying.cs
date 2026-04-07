using UnityEngine;
using UnityEngine.Rendering;

public class deploying : CAVE2Interactable
{
    [Header("Deploy Settings")]
    [SerializeField] CAVE2.Button deployButton = CAVE2.Button.Button3;
    [SerializeField] GameObject prefabToDeploy;

    [Header("Audio Settings")]
    [SerializeField] AudioClip[] deployLoopSounds;

    [Header("Linked Environment")]
    [Tooltip("Drag the specific invisible coral group for this area here.")]
    [SerializeField] SpeakerReefTrigger linkedReef; 

    public static int totalDeployedCount = 0;

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == deployButton)
        {
            Transform wand = CAVE2.GetWandObject(evt.wandID).transform;
            deployableItem heldItem = Camera.main.GetComponentInChildren<deployableItem>();

            if (heldItem != null && heldItem.IsGrabbed)
            {
                heldItem.Consume();

                if (prefabToDeploy != null)
                {
                    Vector3 spawnPosition = transform.position;

                    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
                    {
                        spawnPosition = hit.point;
                    }

                    GameObject spawnedObject = Instantiate(prefabToDeploy, spawnPosition, transform.rotation);
                    AttachLoopingAudio(spawnedObject);
                    totalDeployedCount++;

                    // instead of instantly growing reef, add listener to speaker
                    if (linkedReef != null)
                    {
                        CoralVolumeListener listener = spawnedObject.AddComponent<CoralVolumeListener>();
                        listener.linkedReef = linkedReef; 
                    }
                    else
                    {
                        Debug.LogWarning("Speaker deployed, but no Reef is linked in the Inspector!");
                    }
                    
                }

                Destroy(gameObject);
            }
        }
    }

    void AttachLoopingAudio(GameObject target)
    {
        if (deployLoopSounds.Length == 0) return;
        
        int soundIndex = Mathf.Clamp(totalDeployedCount, 0, deployLoopSounds.Length - 1);
        AudioClip clipToPlay = deployLoopSounds[soundIndex];

        if (clipToPlay != null)
        {
            AudioSource source = target.AddComponent<AudioSource>();
            source.clip = clipToPlay;
            source.loop = true;
            
            source.volume = 0.0f;      
            source.spatialBlend = 1.0f;     
            
            source.minDistance = 5.0f;     
            source.maxDistance = 30.0f;     
            
            source.rolloffMode = AudioRolloffMode.Logarithmic; 
          
            source.playOnAwake = true;
            source.Play();
        }
    }
}