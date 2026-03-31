using UnityEngine;

public class deploying : CAVE2Interactable
{
    [Header("Deploy Settings")]
    [SerializeField] CAVE2.Button deployButton = CAVE2.Button.Button3;
    [SerializeField] GameObject prefabToDeploy;

    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == deployButton)
        {
            Transform wand = CAVE2.GetWandObject(evt.wandID).transform;
            deployableItem heldItem = wand.GetComponentInChildren<deployableItem>();

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


                    Instantiate(prefabToDeploy, spawnPosition, transform.rotation);
                }

                Destroy(gameObject);
            }
        }
    }
}