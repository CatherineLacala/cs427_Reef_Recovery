using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSwitch : MonoBehaviour
{
    public Material sky1;          // Skybox for underwater
    public Material sky2;          // Skybox for above water
    public Transform waterSurface;
    public Transform head;

    private bool isUnderwater = false;

    void Update()
    {
        // if head is below water surface
        if (head.position.y < waterSurface.position.y)
        {
            // switch to sky1
            if (!isUnderwater)
            {
                RenderSettings.skybox = sky1;
                isUnderwater = true;
            }
        }
        else
        {
            // switch to sky2
            if (isUnderwater)
            {
                RenderSettings.skybox = sky2;
                isUnderwater = false;
            }
        }
    }
}