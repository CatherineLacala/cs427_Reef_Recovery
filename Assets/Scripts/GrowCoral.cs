using System.Collections;
using System.Collections.Generic;
//using System.Numerics;
//using System.Threading.Tasks.Dataflow;
using UnityEngine;

public class GrowCoral : MonoBehaviour
{
    [Header("Wait Settings")]
    [SerializeField] int targetDeployCount = 2;
    [SerializeField] float delayAfterAllDeployed = 5f;
    [SerializeField] AudioClip milestoneAudioClip;
    [SerializeField] AudioSource milestoneAudioSource;

    [Header("Growth Settings")]
    public float delayStartGrow = 120f; //waiting 2 minutes before growing
    //public float growSpeed = 0.1f; //speed of how fast the coral grows
    public float growDuration = 5f; //takes 5 seconds for coral to grow
    public float randomDelayMin = 60f;
    public float randomDelayMax = 180f;
    public float growingHeight = 10f; //how far up the coral moves
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartGrowth());
    }

    IEnumerator StartGrowth()
    {
        while (deploying.totalDeployedCount < targetDeployCount || !firstSpeakerSound.firstIsPlayed)
        {
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(delayAfterAllDeployed);

        if (milestoneAudioClip != null && milestoneAudioSource != null)
        {
            milestoneAudioSource.PlayOneShot(milestoneAudioClip);
        }


        foreach (Transform coral in transform)
        {
            StartCoroutine(GrowSingleCoral(coral));


            // TODO: spawn fish
        }
    }

    IEnumerator GrowSingleCoral(Transform coral)
    {
        //random delay so coral doesn't grow all at the same time
        float delay = Random.Range(randomDelayMin, randomDelayMax);
        yield return new WaitForSeconds(delay);

        Vector3 startPosition = coral.position;
        Vector3 targetPosition = startPosition + Vector3.up * growingHeight;

        // float time = 0f;
        // while (time < 1f)
        // {
        //     time += Time.deltaTime * growSpeed;
        //     coral.position = Vector3.Lerp(startPosition, targetPosition, time);
        //     yield return null;
        // }

        float elapsed = 0f;
        while (elapsed < growDuration)
        {
            elapsed += Time.deltaTime;
            float time = elapsed / growDuration; //going from 0 to 1 over 5 seconds
            coral.position = Vector3.Lerp(startPosition, targetPosition, time);
            yield return null;
        }

        coral.position = targetPosition; //coral is in final position
    }

}
