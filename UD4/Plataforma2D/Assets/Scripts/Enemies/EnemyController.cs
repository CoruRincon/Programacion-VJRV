using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] float speed; // Referencia a la velocidad de movimiento
    [SerializeField] Rigidbody2D rb; // Referencia al Rigidbody2D
    [SerializeField] float distance; // Referencia a la distancia máxima de movimiento
    Vector2 originalPosition; // Posición original del enemigo
    [SerializeField]
    bool rightMotion = true;

    [SerializeField]
    float rayLength = 1f;
    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    float heightOffset;

    bool stomped = false;

    [SerializeField]
    string deadSound;

    public void Start() {
        // Obtenemos la posición original del enemigo
        originalPosition = transform.position;
    }

    // Esta función se ejecuta cada X tiempo
    public void FixedUpdate() {
        Vector2 raycastOrigin = new Vector2(gameObject.transform.position.x, transform.position.y);

        RaycastHit2D hit;

        // Dibujamos una línea de color negro hacia abajo que parta desde la posición calculada anteriormente
        // y que termine en donde indique el segundo parámetro del método DrawLine()
        if (rightMotion) {
            hit = Physics2D.Raycast(raycastOrigin, Vector2.right, rayLength, groundLayer);
        }
        else {
            hit = Physics2D.Raycast(raycastOrigin, Vector2.left, rayLength, groundLayer);
        }

        if(hit)
            rightMotion = !rightMotion;

        // Si la posición original del enemigo sumada a la distancia máxima
        // es inferior a la posición actual el enemigo sigue moviéndose a la derecha.
        if ((transform.position.x < (originalPosition.x + distance)) && rightMotion) {
            rb.linearVelocity = new Vector2(speed, rb.linearVelocityY);
        }
        else if ((transform.position.x >= (originalPosition.x + distance)) && rightMotion) rightMotion = false;
        // Si la posición original del enemigo restándole la distancia máxima
        // es inferior a la posición actual el enemigo sigue moviéndose a la izquierda.
        if ((transform.position.x > (originalPosition.x - distance)) && !rightMotion) {
            rb.linearVelocity = new Vector2(-speed, rb.linearVelocityY);
        }
        else if ((transform.position.x <= (originalPosition.x - distance)) && !rightMotion) rightMotion = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Player" && !stomped) {
            if(!other.transform.GetComponent<PlayerController>().enabled) {
                return;
            }

            Vector2 playerPos = other.transform.position;

            if (playerPos.y <= transform.position.y + heightOffset && !other.transform.GetComponent<PlayerController>().hasStarPower) {
                Debug.Log("Dead");
                GetComponent<CompositeCollider2D>().isTrigger = true;
                AudioManager.Instance.PlayEffect("PlayerHit", false);
                other.transform.GetComponent<PlayerController>().Die();
                SCManager.Instance.LoadScene("GameOver", 5.0f);
                rightMotion = !rightMotion;
                return; 
            }

            if (TryGetComponent<Animator>(out Animator anim)) {
                anim.SetTrigger("stomped");
                AudioManager.Instance.PlayEffect(deadSound, false);
                speed = 0;
                stomped = true;
                gameObject.layer = LayerMask.NameToLayer("Ground");

                GetComponent<BoxCollider2D>().size = new Vector2(GetComponent<BoxCollider2D>().size.x, 0.45f);
                GetComponent<BoxCollider2D>().offset = new Vector2(GetComponent<BoxCollider2D>().offset.x, -.25f);
                Destroy(gameObject, 1.5f);
            }
        }
    }
}
