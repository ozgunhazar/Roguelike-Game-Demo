using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    
    [SerializeField] private Button m_PlayButton;
    [SerializeField] private Button m_QuitButton;


    private void Awake() {
        
        m_PlayButton.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.GameScene);
        });
        m_QuitButton.onClick.AddListener(() => {
            Application.Quit();
        });
        
    }

    
    
}
