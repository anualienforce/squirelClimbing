using UnityEngine;

public class PanelController : MonoBehaviour
{
    [SerializeField] private GameObject panel; // drag your panel here

    // Call this on Button Click to OPEN
    public void OpenPanel()
    {
        if (panel != null)
            panel.SetActive(true);
    }

    // Call this on Button Click to CLOSE
    public void ClosePanel()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}
