using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitcher : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    
    
    // Switches to the specified level
    public void SwitchLevel(string levelName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }

    // Quits the application
    public void QuitGame()
    {
        Application.Quit();
    }

    // Opens the settings menu and closes the main menu
    public void OpenSettings()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }

    // Closes the settings menu and returns to the main menu
    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    // Opens the credits menu and closes the main menu
    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        creditsMenu.SetActive(true);
    }

    // Closes the credits menu and returns to the main menu
    public void CloseCredits()
    {
        creditsMenu.SetActive(false);
        mainMenu.SetActive(true);
    }
}
