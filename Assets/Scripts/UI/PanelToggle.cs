using UnityEngine;

public class PanelToggle : MonoBehaviour
{
    [Tooltip("The panel to show/hide.")]
    [SerializeField] private GameObject panelToToggle;


    // Toggles the active state of the assigned panel.

    public void TogglePanel()
    {
        if (panelToToggle != null)
        {
            bool isActive = panelToToggle.activeSelf;
            panelToToggle.SetActive(!isActive);
        }
        else
        {
            Debug.LogWarning("PanelToggle: No panel assigned to toggle.");
        }
    }
}
