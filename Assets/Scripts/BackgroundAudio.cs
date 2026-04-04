using UnityEngine;

public class BackgroundAudio : MonoBehaviour
{
    public Transform player;
    public Transform waterSurfaceMirror;
    public int wandID = 1;

    public AudioSource oneShotSource;

    public AudioSource aboveWaterSource;
    public AudioSource oceanWavesSource;
    public AudioSource scubaBreathingSource;
    public AudioSource walkOnSandSource;
    public AudioSource walkOnWoodSource;

    public AudioClip aboveWaterNoise;
    public AudioClip oceanWaves;
    public AudioClip scubaBreathing;
    public AudioClip walkOnSand;
    public AudioClip walkOnWood;

    public float wavesBelowSurfaceDistance = 2f;
    public float movementThreshold = 0.1f;

    [Header("Volume")]
    [Range(0f, 1f)] public float masterVolume = 0.5f;
    public float volumeStep = 0.05f;

    private float waterSurfaceY;
    private Vector3 lastPlayerPosition;

    void Start()
    {
        if (player == null)
        {
            player = transform;
        }

        if (waterSurfaceMirror != null)
        {
            waterSurfaceY = waterSurfaceMirror.position.y;
        }
        else
        {
            Debug.LogWarning("BackgroundAudio: No waterSurfaceMirror assigned.");
        }

        SetupSource(aboveWaterSource, aboveWaterNoise);
        SetupSource(oceanWavesSource, oceanWaves);
        SetupSource(scubaBreathingSource, scubaBreathing);
        SetupSource(walkOnSandSource, walkOnSand);
        SetupSource(walkOnWoodSource, walkOnWood);

        ApplyVolumeToAllSources();

        lastPlayerPosition = player.position;
    }

    void Update()
    {
        HandleVolumeInput();

        if (player == null || waterSurfaceMirror == null) return;

        waterSurfaceY = waterSurfaceMirror.position.y;

        float playerY = player.position.y;

        bool isAboveSurface = playerY > waterSurfaceY;
        bool isNearSurface = playerY > waterSurfaceY - wavesBelowSurfaceDistance;
        bool isBelowSurface = playerY < waterSurfaceY;

        SetSourcePlaying(aboveWaterSource, isAboveSurface);
        SetSourcePlaying(oceanWavesSource, isNearSurface);
        SetSourcePlaying(scubaBreathingSource, isBelowSurface);

        bool isMoving = IsPlayerMoving();
        bool onTerrain = IsPlayerOnTerrain();
        bool onWoodDock = IsPlayerOnWoodDock();

        SetSourcePlaying(walkOnSandSource, onTerrain && isMoving && !onWoodDock);
        SetSourcePlaying(walkOnWoodSource, onWoodDock && isMoving);

        lastPlayerPosition = player.position;
    }

    bool IsPlayerOnWoodDock()
    {
        Ray ray = new Ray(player.position + Vector3.up * 0.2f, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hit, 3f))
        {
            return hit.collider.CompareTag("WoodDocks");
        }

        return false;
    }

    void HandleVolumeInput()
    {
        // CAVE2 Wand
        if (CAVE2.GetButtonDown(CAVE2.Button.ButtonRight, wandID) || Input.GetKeyDown(KeyCode.Equals))
        {
            masterVolume = Mathf.Clamp01(masterVolume + volumeStep);
            ApplyVolumeToAllSources();
        }

        if (CAVE2.GetButtonDown(CAVE2.Button.ButtonLeft, wandID) || Input.GetKeyDown(KeyCode.Minus))
        {
            masterVolume = Mathf.Clamp01(masterVolume - volumeStep);
            ApplyVolumeToAllSources();
        }
    }

    void ApplyVolumeToAllSources()
    {
        SetVolume(aboveWaterSource);
        SetVolume(oceanWavesSource);
        SetVolume(scubaBreathingSource);
        SetVolume(walkOnSandSource);
        SetVolume(oneShotSource);
        SetVolume(walkOnWoodSource);
    }

    void SetVolume(AudioSource source)
    {
        if (source != null)
        {
            source.volume = masterVolume;
        }
    }

    void SetupSource(AudioSource source, AudioClip clip)
    {
        if (source == null) return;

        source.clip = clip;
        source.loop = true;
        source.playOnAwake = false;
        source.volume = masterVolume;
    }

    void SetSourcePlaying(AudioSource source, bool shouldPlay)
    {
        if (source == null || source.clip == null) return;

        if (shouldPlay)
        {
            if (!source.isPlaying)
            {
                source.Play();
            }
        }
        else
        {
            if (source.isPlaying)
            {
                source.Stop();
            }
        }
    }

    bool IsPlayerOnTerrain()
    {
        Ray ray = new Ray(player.position + Vector3.up * 0.2f, Vector3.down);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 3f))
        {
            return hit.collider.GetComponent<TerrainCollider>() != null;
        }

        return false;
    }

    bool IsPlayerMoving()
    {
        Vector3 horizontalDelta = player.position - lastPlayerPosition;
        horizontalDelta.y = 0f;

        return horizontalDelta.magnitude > movementThreshold * Time.deltaTime;
    }
}