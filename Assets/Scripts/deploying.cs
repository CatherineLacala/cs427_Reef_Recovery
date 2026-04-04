using UnityEngine;
using UnityEngine.Rendering;

public class deploying : CAVE2Interactable
{
    [Header("Deploy Settings")]
    [SerializeField] CAVE2.Button deployButton = CAVE2.Button.Button3;
    [SerializeField] GameObject prefabToDeploy;

    [Header("Audio Settings")]
    [SerializeField] AudioClip[] deployLoopSounds;

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

                    // shoot a raycast down
                    if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit))
                    {
                        // when we hit the ground, make that where it spawns
                        spawnPosition = hit.point;
                    }

                    GameObject spawnedObject = Instantiate(prefabToDeploy, spawnPosition, transform.rotation);
                    AttachLoopingAudio(spawnedObject);
                    totalDeployedCount++;
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
            //AudioSource source = target.AddComponent<AudioSource>();
            //source.clip = clipToPlay;
            //source.loop = true;
            //source.spatialBlend = 1.0f;
            //source.minDistance = 1.0f;
            //source.playOnAwake = true;
            //source.Play();
        }
    }
}