using UnityEngine;

public class ToolSwitcher : MonoBehaviour
{
    [Header("ToolKit")]
    public GameObject[] toolIcons;
    public GameObject[] toolOutlines;
    private int currentIndex = 0;

    void Start()
    {
        currentIndex = toolIcons.Length;
        UpdateToolUI();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            SwitchNext();
        }
    }

    public void SwitchNext()
    {
        currentIndex = (currentIndex + 1) % (toolIcons.Length + 1);
        UpdateToolUI();
    }

    void UpdateToolUI()
    {
        bool isBareHanded = (currentIndex == toolIcons.Length);
        for (int i = 0; i < toolIcons.Length; i++)
        {
            bool shouldShow = !isBareHanded && (i == currentIndex);
            toolOutlines[i]?.SetActive(shouldShow);
        }
        if (isBareHanded)
        {
            Debug.Log("NoTool");
        }
        else
        {
            Debug.Log("Switched to:" + toolIcons[currentIndex].name);
        }
    }
}