using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatTrigger : MonoBehaviour
{
    public BoatAudio audioManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlaySolarPanelAudio();
        }
    }
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // // Update is called once per frame
    // void Update()
    // {
        
    // }
}
