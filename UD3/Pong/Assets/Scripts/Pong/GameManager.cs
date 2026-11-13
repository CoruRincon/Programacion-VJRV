using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public enum GameState {
        PLAYING,
        PAUSE,
        AWAIT
    }

    [SerializeField]
    private GameState currentState;

    private int playerScore, npcScore;

    [SerializeField]
    private GameObject ball, PaddleL, PaddleR;

    [SerializeField]
    private TextMeshProUGUI scoreBoard;

    private void Awake() {
        if(Instance == null)
            Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = GameState.AWAIT;

        playerScore = 0;
        npcScore = 0;

        ball = GameObject.FindWithTag("Ball");
    }

    // Update is called once per frame
    void Update()
    {
        if(ball  == null) { return; }

        if (currentState == GameState.PLAYING) {
            if (!PaddleL.GetComponent<NPCController>().enabled)
                PaddleL.GetComponent<NPCController>().enabled = true;
        }

        if (currentState == GameState.AWAIT) {
            if (Input.anyKeyDown) {
                PaddleL.GetComponent<NPCController>().enabled = false;
                ChangeGameState(GameManager.GameState.PLAYING);
                ball.GetComponent<BallMotion>().LaunchBall();
            }
        }

        scoreBoard.text = $"{npcScore} | {playerScore}";
    }

    public void ChangeGameState(GameState state) {
        currentState = state;
    }

    public void Score(bool isPlayer) {
        if (isPlayer)
            playerScore++;
        else
            npcScore++;

        PaddleL.transform.position = new Vector3(PaddleL.transform.position.x, 0, PaddleL.transform.position.z);
        PaddleR.transform.position = new Vector3(PaddleR.transform.position.x, 0, PaddleR.transform.position.z);
    }
}
