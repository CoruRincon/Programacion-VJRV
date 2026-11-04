using UnityEngine;

public class BallMotion : MonoBehaviour {

    [SerializeField]
    private float ballSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void LaunchBall() {
        int direction = Random.Range(0, 4);
        Vector2 motion = Vector2.zero;

        switch (direction) {
            case 0:
                motion = new Vector2(ballSpeed, ballSpeed);
                break;

            case 1:
                motion = -new Vector2(ballSpeed, ballSpeed);
                break;

            case 2:
                motion = new Vector2(-ballSpeed, ballSpeed);
                break;

            case 3:
                motion = new Vector2(-ballSpeed, -ballSpeed);
                break;
        }

        GetComponent<Rigidbody2D>().linearVelocity = motion;
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D other) {
        if (other.gameObject.tag != "ScoreLine") { return; }

        if (transform.position.x < 0)
            GameManager.Instance.Score(true);
        else
            GameManager.Instance.Score(false);

        GameManager.Instance.ChangeGameState(GameManager.GameState.AWAIT);

        transform.position = Vector3.zero;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag != "GameArea") { return; }

        Debug.Log("Out of playing area");

        GameManager.Instance.ChangeGameState(GameManager.GameState.AWAIT);

        transform.position = Vector3.zero;

        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
    }
}
