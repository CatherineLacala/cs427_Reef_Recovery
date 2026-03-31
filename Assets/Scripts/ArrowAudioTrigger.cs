using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ArrowAudioTrigger : MonoBehaviour
{
    // bobbing like an apple not the guy Bob
    public float bobSpeed = 3f;
    public float bobHeight = 0.25f;

    public Transform player;
    public float triggerDistance = 5f;
    public AudioClip proximityClip;

    private AudioSource audioSource;
    private Vector3 startPos;
    private bool hasPlayedAudio = false;

    void Start()
    {
        startPos = transform.position;

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }
    }

    void Update()
    {
        if (hasPlayedAudio || player == null) return;

        float newY = startPos.y + (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        transform.position = new Vector3(startPos.x, newY, startPos.z);

        // close enough?
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance)
        {
            if (!hasPlayedAudio && proximityClip != null)
            {
                audioSource.PlayOneShot(proximityClip);
                hasPlayedAudio = true;

                HideAndCleanupArrow();
            }
        }
    }

    private void HideAndCleanupArrow()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }

        if (proximityClip != null)
        {
            Destroy(gameObject, proximityClip.length);
        }
    }
}