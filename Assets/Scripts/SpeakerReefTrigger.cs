using System.Collections;
using UnityEngine;

public class SpeakerReefTrigger : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public Transform coralParent;   // Coral group for THIS speaker
    public GameObject fishGroup;    // Fish group for THIS speaker

    [Header("Growth Settings")]
    public float growDuration = 5f;
    public float growingHeight = 10f;
    public float randomDelayMin = 1f;
    public float randomDelayMax = 3f;

    private bool activated = false;

    // Call this when the speaker is placed
    public void OnSpeakerPlaced()
    {
        if (!activated)
        {
            activated = true;
            StartCoroutine(ActivateReef());
        }
    }

    IEnumerator ActivateReef()
    {
        // Grow coral
        foreach (Transform coral in coralParent)
        {
            StartCoroutine(GrowSingleCoral(coral));
        }

        // Turn on fish after a short delay
        yield return new WaitForSeconds(1.5f);
        fishGroup.SetActive(true);
    }

    IEnumerator GrowSingleCoral(Transform coral)
    {
        //random delay so coral doesn't grow all at the same time
        float delay = Random.Range(randomDelayMin, randomDelayMax);
        yield return new WaitForSeconds(delay);

        Vector3 startPosition = coral.position;
        Vector3 targetPosition = startPosition + Vector3.up * growingHeight;

        float elapsed = 0f;
        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / growDuration;  //going from 0 to 1 over 5 seconds
            coral.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        coral.position = targetPosition;  //coral is in final position
    }
}