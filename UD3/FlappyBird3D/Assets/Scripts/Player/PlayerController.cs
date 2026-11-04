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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb == null) return;

        if (Input.GetKeyDown(KeyCode.Space)) {
            rb.linearVelocityY = jumpForce;
        }

        UpdateModel();

        Vector2 screenPosition = Camera.main.WorldToScreenPoint(transform.position);
        if (screenPosition.y > Screen.height || screenPosition.y < 0) {
            Debug.Log("Out of bounds");
        }
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
}
