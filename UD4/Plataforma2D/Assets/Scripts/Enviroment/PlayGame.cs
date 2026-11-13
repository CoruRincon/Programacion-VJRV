using UnityEngine;

public class PlayGame : MonoBehaviour
{

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)){
            SCManager.Instance.LoadScene("Game");
        }
        
    }
}
