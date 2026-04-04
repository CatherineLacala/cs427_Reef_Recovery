using UnityEngine;

public class UnderwaterParticles : MonoBehaviour
{
    public Transform player;
    public Transform waterSurface;

    public ParticleSystem particles;

    private bool isUnderwater = false;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
        }

        if (particles != null)
        {
            particles.Stop();
        }
    }

    void Update()
    {
        if (player == null || waterSurface == null || particles == null) return;

        bool nowUnderwater = player.position.y < waterSurface.position.y;

        if (nowUnderwater != isUnderwater)
        {
            isUnderwater = nowUnderwater;

            if (isUnderwater)
            {
                particles.Play();
            }
            else
            {
                particles.Stop();
            }
        }
    }
}