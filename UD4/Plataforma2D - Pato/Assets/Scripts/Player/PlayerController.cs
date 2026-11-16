using System;
using System.Collections;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    private SpriteRenderer sr;
    Rigidbody2D rb;

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

    public bool hasStarPower;

    bool mapActive = false;

    private readonly Color[] rainbow =
    {
        Color.red,
        new Color(1f, 0.5f, 0f),   // Orange
        Color.yellow,
        Color.green,
        Color.blue,
        new Color(0.29f, 0f, 0.51f), // Indigo
        new Color(0.56f, 0f, 1f)     // Violet
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
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

        if (xMotion > 0.2f) {
            sr.flipX = false;
        }
        else if (xMotion < -0.2f) {
            sr.flipX = true;
        }


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

        RaycastHit2D hit = Physics2D.Raycast(raycastOrigin, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitRight = Physics2D.Raycast(raycastOrigin - Vector2.right, Vector2.down, rayLength, groundLayer);
        RaycastHit2D hitLeft = Physics2D.Raycast(raycastOrigin - Vector2.left, Vector2.down, rayLength, groundLayer);

        if (hit || hitLeft || hitRight) {
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

        rb.bodyType = RigidbodyType2D.Static;

        this.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        switch(other.gameObject.tag) {
            case "PowerUp":
                other.enabled = false;
                Destroy(other.gameObject);
                StartCoroutine(StarPowerUpTimer());
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

    private IEnumerator StarColors() {
        int index = 0;
        WaitForSeconds wait = new WaitForSeconds(0.05f);

        while (hasStarPower) {
            sr.color = rainbow[index];
            index = (index + 1) % rainbow.Length;
            yield return wait;
        }
    }

    private IEnumerator StarPowerUpTimer() {
        hasStarPower = true;
        AudioManager.Instance.PlayMusic("StarMusic");

        StartCoroutine(StarColors());

        yield return new WaitForSeconds(8.5f);
        AudioManager.Instance.PlayMusic("GameMusic");
        hasStarPower = false;

        StopCoroutine(StarColors());

        sr.color = Color.white;
    }
}
