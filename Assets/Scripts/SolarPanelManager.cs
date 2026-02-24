using UnityEngine;

public class SolarPanelManager : MonoBehaviour
{
    [Header("To Show")]
    public GameObject panels;
    public GameObject panelPillars;
    public GameObject successText;

    [Header("To Hide")]
    public GameObject setUpSolarPanelButton;

    public void SetupSolarPanels()
    {
        if (panels != null) panels.SetActive(true);
        if (panelPillars != null) panelPillars.SetActive(true);

        // 2. Show the TextMeshPro text
        if (successText != null) successText.SetActive(true);

        // 3. Hide the button so it can't be clicked again
        if (setUpSolarPanelButton != null) setUpSolarPanelButton.SetActive(false);
    }
}