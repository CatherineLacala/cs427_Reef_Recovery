using UnityEngine;
using UnityEngine.Events;

public class CAVE2ButtonTrigger : CAVE2Interactable
{
    public CAVE2.Button clickButton = CAVE2.Button.Button3;

    public UnityEvent onClick;

    // This detects the CAVE2 Wand click
    // Debugging help
    new void OnWandButtonDown(CAVE2.WandEvent evt)
    {
        if (evt.button == clickButton)
        {
            Debug.Log("Wand clicked the button!");
            onClick.Invoke();
        }
    }
}