using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSpeaker : MonoBehaviour
{
    public BlinkingLight blinkingLight;

    public void PressButton()
    {
        if (blinkingLight != null)
        {
            blinkingLight.TurnOffLight();
        }
    }
}
