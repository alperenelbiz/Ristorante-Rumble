using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public Button hostButton;
    public Button joinButton;
    public Button settingsButton;
    

    void Start()
    {
        hostButton.onClick.AddListener(OnHostButtonClicked);
        joinButton.onClick.AddListener(OnJoinButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    void OnHostButtonClicked()
    {
        
        SceneManager.LoadScene("Test"); 
    }

    void OnJoinButtonClicked()
    {
        
        SceneManager.LoadScene("Test"); 
    }

    void OnSettingsButtonClicked()
    {
        SceneManager.LoadScene("Test"); 
    }
}
