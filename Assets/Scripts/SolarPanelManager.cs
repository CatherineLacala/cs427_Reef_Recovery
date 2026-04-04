using UnityEngine;

public class SolarPanelManager : MonoBehaviour
{
    public GameObject panels;
    public GameObject panelPillars;
    public GameObject successText;

    public GameObject setUpSolarPanelButton;

    public AudioSource audioSource;
    public AudioClip setupSound;

    public void SetupSolarPanels()
    {
        if (panels != null) panels.SetActive(true);
        if (panelPillars != null) panelPillars.SetActive(true);

        if (successText != null) successText.SetActive(true);

        if (setUpSolarPanelButton != null) setUpSolarPanelButton.SetActive(false);

        if (audioSource != null && setupSound != null)
        {
            audioSource.PlayOneShot(setupSound);
        }
    }
}