using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CameraController : MonoBehaviour {
    public static CameraController Instance;

    [Header("Camera Settings")]

    [SerializeField]
    private GameObject Camera2D;

    [SerializeField]
    private GameObject Camera3D;



    [Header("UI Settings")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI highScoreText;

    [SerializeField]
    private TMP_FontAsset Font2D;

    [SerializeField]
    private TMP_FontAsset Font3D;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (Instance == null) {
            Instance = this;
        }

        Camera2D.SetActive(true);
        Camera3D.SetActive(false);

        scoreText.font = Font2D;
        highScoreText.font = Font2D;
    }

    // Update is called once per frame
    void Update() {

    }

    public void ChangeCamera() {
        Camera2D.SetActive(!Camera2D.activeInHierarchy);
        Camera3D.SetActive(!Camera3D.activeInHierarchy);

        if (Camera2D.activeInHierarchy) {
            scoreText.font = Font2D;
            highScoreText.font = Font2D;
        } else {
            scoreText.font = Font3D;
            highScoreText.font = Font3D;
        }

        scoreText.UpdateFontAsset();
        highScoreText.UpdateFontAsset();
    }
}
