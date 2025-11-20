using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    
    public static bool IsGamePaused = false;
    public GameObject pauseMenuUI;


    void Update() {

        if (Keyboard.current.escapeKey.wasPressedThisFrame) {

            if (IsGamePaused) {
                Resume();
            } else {
                Pause();
            }
            
        }
        
    }

    public void Resume() {
        
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        IsGamePaused = false;
        
    }

    void Pause() {
        
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
        
    }

    public void LoadMenu() {

        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");

    }

    public void QuitGame() {
        
        Debug.Log("Quitting Game");
        Application.Quit();
        
    }

}
