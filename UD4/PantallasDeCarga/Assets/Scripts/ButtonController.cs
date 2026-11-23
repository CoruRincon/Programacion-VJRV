using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    public void LoadSceneButton(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

}
