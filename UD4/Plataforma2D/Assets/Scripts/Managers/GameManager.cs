using System.Collections;

using UnityEngine;

using TMPro;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public enum GameStates {
        MENU,
        LOADING,
        AWAIT,
        PLAYING,
        PAUSE,
        DEAD
    }

    [SerializeField]
    private GameStates currentState;

    [SerializeField]
    private float score;

    [SerializeField]
    private TextMeshProUGUI scoreText; 

    [SerializeField]
    private GameObject deadPanel; 

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        DontDestroyOnLoad(transform.parent);
    }

    void Start(){
        SCManager.Instance.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update() {
        switch (currentState) {
            case GameStates.MENU:

                break;

            case GameStates.LOADING:

                break;

            case GameStates.AWAIT:
                Time.timeScale = 0.0f;
                UpdateScores();

                if(deadPanel != null)
                    deadPanel.SetActive(false);

                SetGameState(GameStates.PLAYING);

                break;

            case GameStates.PLAYING:
                Time.timeScale = 1.0f;
                    
                UpdateScores();
                break;

            case GameStates.PAUSE:
                Time.timeScale = 0.0f;
                break;

            case GameStates.DEAD:
                Time.timeScale = 0.0f;

                if(deadPanel != null)
                    deadPanel.SetActive(true);
                break;
        }
    }

    private void FindComponents() {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        //deadPanel = GameObject.Find("DeadPanel");
    }

    public void SetGameState(GameStates thisState) {
        this.currentState = thisState;
    }

    public GameStates GetGameState() {
        return currentState;
    }

    public void UpdateScores() {
        scoreText.text = $"Score \n{score}";
    }

    public void AddPoint(int amount) {
        score += amount;
    }
    
    public void InitializeGame() {
        FindComponents();
        score = 0;
        UpdateScores();
        SetGameState(GameStates.AWAIT);
        AudioManager.Instance.PlayMusic("GameMusic");
    }
}


