using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxSwitch : MonoBehaviour
{
    public Material sky1;
    public Material sky2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) {
            RenderSettings.skybox = sky1;
            
        }

    }
}
