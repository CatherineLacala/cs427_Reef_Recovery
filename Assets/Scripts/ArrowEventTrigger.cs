using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArrowEventTrigger : MonoBehaviour
{
    [Header("Animation Settings")]
    public float bobSpeed = 3f;
    public float bobHeight = 0.25f;

    [Header("Trigger Settings")]
    public Transform player;
    public float triggerDistance = 5f;

    [Header("Script")]
    public UnityEvent onPlayerApproach;

    [Header("Cleanup Settings")]
    public bool hideAndDestroy = true;
    public float destroyDelay = 0f;

    private Vector3 startPos;
    private bool hasTriggered = false;

    void Start()
    {
        startPos = transform.position;

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
        if (hasTriggered || player == null) return;
        float newY = startPos.y + (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        transform.position = new Vector3(startPos.x, newY, startPos.z);
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= triggerDistance)
        {
            hasTriggered = true;
            onPlayerApproach?.Invoke();

            if (hideAndDestroy)
            {
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

        Destroy(gameObject, destroyDelay);
    }
}
