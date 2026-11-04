using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float paddleSpeed;

    [Header("Strike Settings")]
    [SerializeField]
    private float strikeDegrees = 45f;

    [SerializeField]
    private float strikeMovement = 5f;

    [SerializeField]
    private float strikeDuration = 5f;
    [SerializeField]
    private PhysicsMaterial2D strikeMaterial;

    [SerializeField]
    bool isRotating = false;

    Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        float strikeDirection = Input.GetAxisRaw("Horizontal");
        if(isRotating) { return; }

        if (strikeDirection < 0) {
            StartCoroutine(launchRotation(1));
        }
        else if (strikeDirection > 0) {
            StartCoroutine(launchRotation(-1));
        }
    }


    void FixedUpdate() {
        float paddleMove = (Input.GetAxisRaw("Vertical") * paddleSpeed);
        if(!isRotating)
            rb.linearVelocityY = paddleMove;
    }

    IEnumerator launchRotation(int direction) {
        isRotating = true;

        rb.freezeRotation = false;
        GetComponent<BoxCollider2D>().sharedMaterial = strikeMaterial;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(startPosition.x - strikeMovement, startPosition.y, startPosition.z);

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, strikeDegrees * direction);

        float elapsed = 0.0f;

        while (elapsed < strikeDuration) {
            elapsed += Time.deltaTime;

            transform.rotation = Quaternion.Lerp(
                startRotation,
                targetRotation,
                elapsed / strikeDuration);

            transform.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                elapsed / strikeDuration);

            yield return null;
        }

        transform.rotation = targetRotation;
        transform.position = targetPosition;

        elapsed = 0.0f;

        while (elapsed < strikeDuration) {
            elapsed += Time.deltaTime;

            transform.rotation = Quaternion.Lerp(
                targetRotation,
                startRotation,
                elapsed / strikeDuration);

            transform.position = Vector3.Lerp(
                targetPosition,
                startPosition,
                elapsed / strikeDuration);

            yield return null;
        }

        transform.rotation = startRotation;
        transform.position = startPosition;

        rb.freezeRotation = true;

        GetComponent<BoxCollider2D>().sharedMaterial = null;

        isRotating = false;

        yield return null;
    }
}
