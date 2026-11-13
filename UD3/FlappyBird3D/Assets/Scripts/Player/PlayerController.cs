using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Component Settings")]
    private Rigidbody2D rb;

    [Header("Movement Settings")]
    [SerializeField]
    private float jumpForce;

    [Header("Visual Settings")]

    [SerializeField]
    private float maxPitch;

    [SerializeField]
    private float minPitch;

    private Vector3 startRotation;

    [SerializeField]
    private bool impulseJump;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;

        if (Input.GetKeyDown(KeyCode.C)) {
            CameraController.Instance.ChangeCamera();
        }

        switch(GameManager.Instance.GetGameState()){
            case GameManager.GameStates.DEAD:
                if (Input.GetKeyDown(KeyCode.R)) {
                    GameManager.Instance.LoadScene("Game");
                }
                return;

            case GameManager.GameStates.AWAIT:
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) {
                    GameManager.Instance.SetGameStates(GameManager.GameStates.PLAYING);
                }
                return;
            
            case GameManager.GameStates.PAUSE:
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    GameManager.Instance.SetGameStates(GameManager.GameStates.PLAYING);
                }
                break;

            case GameManager.GameStates.PLAYING:
                if (Input.GetKeyDown(KeyCode.Escape)) {
                    GameManager.Instance.SetGameStates(GameManager.GameStates.PAUSE);
                }
                break;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            if(impulseJump)
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            else
                rb.linearVelocityY = jumpForce;

            AudioManager.Instance.PlayEffectPlayer("Jump");
        }

        UpdateModel();
    }

    private void UpdateModel() {
        Vector3 currentRotation = transform.eulerAngles;

        Vector3 newRotation = new Vector3(startRotation.x, startRotation.y, currentRotation.z);

        if (rb.linearVelocity.y > 0.5f) {
            newRotation.z = maxPitch;
        }
        else if (rb.linearVelocity.y < -.5f) {
            newRotation.z = minPitch;
        }
        else {
            newRotation.z = 0.0f;
        }

        transform.rotation = Quaternion.Euler(newRotation);

        rb.angularVelocity = 0.0f;
    }

    public void OnCollisionEnter2D(Collision2D other) {
        if(other.transform.tag.Equals("Obstacle")){
            GameManager.Instance.SetGameStates(GameManager.GameStates.DEAD);
            AudioManager.Instance.PlayEffect("Crash");
        }
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.transform.tag.Equals("Obstacle")){
            GameManager.Instance.AddPoint(1);
            AudioManager.Instance.PlayEffect("Score");
        }
    }
}
