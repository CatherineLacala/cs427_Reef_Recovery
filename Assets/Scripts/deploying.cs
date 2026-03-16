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
                    Instantiate(prefabToDeploy, transform.position, transform.rotation);
                }
                Destroy(gameObject);
            }
        }
    }
}