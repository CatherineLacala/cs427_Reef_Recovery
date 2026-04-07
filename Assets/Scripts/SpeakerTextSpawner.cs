using UnityEngine;
using TMPro; // Required to change the text

public class SpeakerTextSpawner : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject floatingTextPrefab;
    
    [Header("Spawn Settings")]
    [Tooltip("How fast words spawn when volume is at 100%")]
    public float maxVolumeSpawnRate = 0.3f; 
    [Tooltip("How fast words spawn when volume is at 10%")]
    public float minVolumeSpawnRate = 2.5f; 

    private AudioSource audioSource;
    private float spawnTimer = 0f;

    void Update()
    {
  
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null) return; 
        }

        if (!audioSource.isPlaying || audioSource.volume <= 0.05f) return;


        float currentSpawnDelay = Mathf.Lerp(minVolumeSpawnRate, maxVolumeSpawnRate, audioSource.volume);

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnDelay)
        {
            spawnTimer = 0f;
            SpawnWord();
        }
    }

    void SpawnWord()
    {

        if (floatingTextPrefab == null) 
        {
            Debug.LogError("The Spawner is trying to work, but the Prefab slot is EMPTY!");
            return;
        }


        string wordToSpawn = "♪"; 
        if (audioSource.clip != null)
        {
            string clipName = audioSource.clip.name;
            if (clipName.Contains("Acoustic Fog")) wordToSpawn = "whoosh";
            else if (clipName.Contains("Lobster")) wordToSpawn = "chirp";
        }

        // ts spams the console so unless shits really fucked dont uncomment
        // Debug.Log("Successfully spawning text: " + wordToSpawn + " at volume " + audioSource.volume);

        Vector3 spawnPos = transform.position + (Vector3.up * 1.5f);
        spawnPos += new Vector3(Random.Range(-0.2f, 0.2f), 0, Random.Range(-0.2f, 0.2f));

        GameObject newWord = Instantiate(floatingTextPrefab, spawnPos, Quaternion.identity);

        TextMeshPro tmpro = newWord.GetComponent<TextMeshPro>();
        if (tmpro != null)
        {
            tmpro.text = wordToSpawn;
        }
    }
}