using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {
    // Update is called once per frame
    [SerializeField]
    float motionVelocity = 1f;

    [SerializeField]
    float fuerzaSalto = 1f;


    [SerializeField]
    float offsetRaycast = 1f;
    [SerializeField]
    float rayLength = 1f;
    [SerializeField]
    Vector2 rayRadius = Vector2.zero;

    [SerializeField]
    LayerMask groundLayer;

    [SerializeField]
    private bool isGrounded;

    [SerializeField]
    private ParticleSystem dustParticles, bloodParticles;

    bool mapActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {

    }

    // Update se llama una vez por cada frame
    private void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
            if (mapActive) {
                SceneManager.UnloadSceneAsync("MapScene");
                mapActive = false;
            } else {
                SceneManager.LoadScene("MapScene", LoadSceneMode.Additive);
                mapActive = true;
            }
        }

        // Lectura del eje de entrada horizontal
        float xMotion = Input.GetAxisRaw("Horizontal");

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (xMotion > 0.2f) {
            sr.flipX = false;
        }
        else if (xMotion < -0.2f) {
            sr.flipX = true;
        }

        // Referencia al Rigidbody2D del gameObject que contiene este script
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Se genera un nuevo vector con la velocidad a aplicar
        Vector2 speedVector = new Vector2(xMotion * motionVelocity, rb.linearVelocityY);

        // Se aplica la velocidad a las físicas del gameObject que contiene el script
        GetComponent<Rigidbody2D>().linearVelocity = speedVector;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            // Aplicamos la fuerza física en la coordenada vertical
            rb.AddForce(Vector2.up * fuerzaSalto, ForceMode2D.Impulse);

            Vector2 dustPos = transform.position;

            dustParticles.transform.position = new Vector2(dustPos.x, dustPos.y - 0.5f);
            dustParticles.Play();

            AudioManager.Instance.PlayEffectPlayer("Jump", true);

            SetAnimation("isJumping");
        }

        float colliderSizeY = gameObject.GetComponent<Collider2D>().bounds.size.y + offsetRaycast;
        Vector2 raycastOrigin = new Vector2(gameObject.transform.position.x, transform.position.y - (colliderSizeY / 2));
        Vector2 raycastEnd = raycastOrigin - (new Vector2(0f, rayLength));

        // Dibujamos una línea de color negro hacia abajo que parta desde la posición calculada anteriormente
        // y que termine en donde indique el segundo parámetro del método DrawLine()
        Debug.DrawLine(raycastOrigin, raycastEnd, Color.black);

        RaycastHit2D hit = Physics2D.CapsuleCast(raycastOrigin, rayRadius, CapsuleDirection2D.Horizontal, 0.0f, Vector2.down, rayLength, groundLayer);
        //RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, rayLength, groundLayer);

        if (hit) {
            isGrounded = true;

            if (Mathf.Abs(xMotion) > 0) {
                SetAnimation("isWalking");
            }
            else {
                SetAnimation("");
            }
        }
        else {
            isGrounded = false;
            SetAnimation("isJumping");
        }
    }

    // -----------------------------------------------------------------------------
    // Método que desactiva todos los parámetros del Animator y activa uno concreto
    // -----------------------------------------------------------------------------
    void SetAnimation(string name) {

        // Obtenemos todos los parámetros del Animator
        AnimatorControllerParameter[] parametros = GetComponent<Animator>().parameters;

        // Recorremos todos los parámetros y los ponemos a false
        foreach (var item in parametros) GetComponent<Animator>().SetBool(item.name, false);

        if(name != "")
            GetComponent<Animator>().SetBool(name, true);

    }

    public void Die() {
        SetAnimation("");
        transform.rotation = Quaternion.Euler(new Vector3 (0f, 0f, 90f));
        Vector2 bloodPos = transform.position;

        bloodParticles.transform.position = new Vector2(bloodPos.x, bloodPos.y - 0.5f);
        bloodParticles.Play();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch(other.gameObject.tag) {
            case "PowerUp":
                AudioManager.Instance.PlayMusic("StarMusic");
                other.enabled = false;
                Destroy(other.gameObject);
                break;

            case "Coin":
                AudioManager.Instance.PlayEffect("Collect", false);
                GameManager.Instance.AddPoint(1);
                other.enabled = false;
                Destroy(other.gameObject);
                break;

            default:
                return;
        }
    }
}
