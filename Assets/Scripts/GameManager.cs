using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour {
    
    public static GameManager Instance { get; private set; }

    public BoardManager BoardManager;
    public PlayerController PlayerController;
    public UIDocument UIDoc;
    private Label m_FoodLabel;
    public int m_CurrentLevel = 1;

    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;

    public TurnManager TurnManager { get; private set; }

    private int m_FoodAmount = 100;

    void Awake() {
        
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }
    
    void Start() {
        
        TurnManager = new TurnManager();
        TurnManager.OnTick += OnTurnHappen;

        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = UIDoc.rootVisualElement.Q<Label>("GameOverMessage");
        
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        
        StartNewGame();
        
    }

    void OnTurnHappen() {

        ChangeFood(-1);

    }

    public void ChangeFood(int amount) {

        m_FoodAmount += amount;
        m_FoodLabel.text = "Food: " + m_FoodAmount;

        if (m_FoodAmount <= 0) {
            PlayerController.GameOver();
            m_GameOverPanel.style.visibility = Visibility.Visible;
            m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " level(s)\n\nPress 'Enter' to start a new game";
        }

    }

    public void NewLevel() {
        
        BoardManager.Clean();
        BoardManager.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

        m_CurrentLevel += 1;
        
        Debug.Log("Current Level: " + m_CurrentLevel);

    }

    public void StartNewGame() {

        m_GameOverPanel.style.visibility = Visibility.Hidden;

        m_CurrentLevel = 1;
        m_FoodAmount = 100;
        m_FoodLabel.text = "Food: " + m_FoodAmount;
        
        BoardManager.Clean();
        BoardManager.Init();

        PlayerController.Init();
        PlayerController.Spawn(BoardManager, new Vector2Int(1, 1));

    }
    
}
