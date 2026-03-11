using UnityEngine;
using System.Collections; // Required for IEnumerator

public class BlinkingLight : MonoBehaviour
{
    private Light myLight;
    public float blinkInterval = 0.5f; // Time in seconds between blinks
    private Coroutine blinkCoroutine;
    void Start()
    {
        // Get the Light component attached to this GameObject
        myLight = GetComponent<Light>();
        // Start the blinking coroutine
        blinkCoroutine = StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true) // Infinite loop to keep blinking
        {
            myLight.enabled = !myLight.enabled; // Toggle the light on/off
            yield return new WaitForSeconds(blinkInterval); // Wait for the specified interval
        }
    }

    // when button clicked, turn off light
    public void TurnOffLight()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }

        myLight.enabled = false;
    }
}
