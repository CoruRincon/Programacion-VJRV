using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float highScore;

    [SerializeField]
    private TextMeshProUGUI scoreText; 
    
    [SerializeField]
    private TextMeshProUGUI highScoreText; 
    
    [SerializeField]
    private GameObject awaitPanel; 

    [SerializeField]
    private GameObject deadPanel; 

    void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        DontDestroyOnLoad(transform.parent);
    }

    void Start(){
        LoadScene("Game");
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

                if(awaitPanel != null)
                    awaitPanel.SetActive(true);
                if(deadPanel != null)
                    deadPanel.SetActive(false);

                break;

            case GameStates.PLAYING:
                Time.timeScale = 1.0f;

                if(awaitPanel != null)
                    awaitPanel.SetActive(false);
                    
                UpdateScores();
                break;

            case GameStates.PAUSE:
                Time.timeScale = 0.0f;
                break;

            case GameStates.DEAD:
                Time.timeScale = 0.0f;

                if(deadPanel != null)
                    deadPanel.SetActive(true);

                if(score > highScore){
                    highScore = score;
                    UpdateScores();
                }
                break;
        }
    }

    private void FindComponents() {
        scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
        highScoreText = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
        awaitPanel = GameObject.Find("AwaitPanel");
        deadPanel = GameObject.Find("DeadPanel");
    }

    public void SetGameStates(GameStates thisState) {
        this.currentState = thisState;
    }

    public GameStates GetGameState() {
        return currentState;
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(SceneLoader(sceneName));
    }

    private IEnumerator SceneLoader(string sceneName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone) {
            // Optionally, you can display loading progress here
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading progress: " + (progress * 100) + "%");

            // Activate the scene when it's ready
            if (asyncLoad.progress >= 0.9f) {
                asyncLoad.allowSceneActivation = true;
            }
            
            yield return null; // Wait for the next frame
        }

        if (sceneName == "Game") {
            FindComponents();
            score = 0;
            UpdateScores();
            SetGameStates(GameStates.AWAIT);
        }

    }

    public void UpdateScores() {
        highScoreText.text = $"High Score \n{highScore}";
        scoreText.text = $"Score \n{score}";
    }

    public void AddPoint(int amount) {
        score += amount;
    }
}
