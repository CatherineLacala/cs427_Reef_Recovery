using UnityEngine;
using System.Collections;

public class BoatTransitManager : MonoBehaviour
{
    [Header("Player & Boat References")]
    public GameObject playerController;
    public GameObject boat;
    public Transform playerBoatPosition;
    public Transform boatDropoff;

    [Header("Button References")]
    public GameObject headToReefButton;

    [Header("Settings")]
    public float transitDuration = 5.0f;

    private bool hasClicked = false;

    public void StartBoatTransit()
    {
        if (hasClicked) return;
        hasClicked = true;

        StartCoroutine(TransitSequence());
    }

    private IEnumerator TransitSequence()
    {
        headToReefButton.SetActive(false);

        TogglePlayerMovement(false);

        playerController.transform.position = playerBoatPosition.position;
        playerController.transform.rotation = playerBoatPosition.rotation;
        playerController.transform.SetParent(boat.transform);

        Vector3 startPos = boat.transform.position;
        Quaternion startRot = boat.transform.rotation;
        float elapsedTime = 0f;

        while (elapsedTime < transitDuration)
        {
            boat.transform.position = Vector3.Lerp(startPos, boatDropoff.position, elapsedTime / transitDuration);
            boat.transform.rotation = Quaternion.Slerp(startRot, boatDropoff.rotation, elapsedTime / transitDuration);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        boat.transform.position = boatDropoff.position;
        boat.transform.rotation = boatDropoff.rotation;

        playerController.transform.SetParent(null);

        TogglePlayerMovement(true);
    }

    private void TogglePlayerMovement(bool canMove)
    {
        CharacterController cc = playerController.GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = canMove;
        }

        Rigidbody rb = playerController.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = !canMove;
        }
    }
}