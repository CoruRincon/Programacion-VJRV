using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance;

    public enum GameState {
        MENU,
        LOADING,
        AWAIT,
        PLAYING,
        PAUSE
    }



    [SerializeField]
    private GameState currentState;

    [SerializeField]
    private float highScore;

    [SerializeField]
    private bool isPaused;

    void Start() {
        if (Instance == null) {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update() {
        switch (currentState) {
            case GameState.MENU:

                break;

            case GameState.LOADING:

                break;

            case GameState.AWAIT:

                break;

            case GameState.PLAYING:

                break;

            case GameState.PAUSE:

                break;
        }
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
    }
}
