using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject openPanelButton;
    public GameObject panel;
    public GameObject closePanelButton;

    void Start()
    {
       
        panel.SetActive(false);
        closePanelButton.SetActive(false);

        
        openPanelButton.GetComponent<Button>().onClick.AddListener(OpenPanel);
        closePanelButton.GetComponent<Button>().onClick.AddListener(ClosePanel);
    }

    void OpenPanel()
    {
        panel.SetActive(true);
        closePanelButton.SetActive(true);
        openPanelButton.SetActive(false);
    }

    void ClosePanel()
    {
        panel.SetActive(false);
        closePanelButton.SetActive(false);
        openPanelButton.SetActive(true);
    }
}
