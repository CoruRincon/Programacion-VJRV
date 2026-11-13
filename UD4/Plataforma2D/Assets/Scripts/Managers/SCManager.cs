using System.Collections;
using UnityEngine;

using UnityEngine.SceneManagement;

public class SCManager : MonoBehaviour {
    public static SCManager Instance;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void LoadScene(string sceneName) {
        StartCoroutine(SceneLoader(sceneName, 0.0f));
    }
    public void LoadScene(string sceneName, float delay) {
        StartCoroutine(SceneLoader(sceneName, delay));
    }

    private IEnumerator SceneLoader(string sceneName, float delay) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false;

        new WaitForSeconds(delay);

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
            GameManager.Instance.InitializeGame();
        }
    }
}
