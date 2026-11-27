using System.Collections;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField]
    float jumpBoost;

    [SerializeField]
    float popDelay;

    [SerializeField]
    Sprite upSprite, downSprite;

    [SerializeField]
    Vector2 colliderSizeDown, colliderSizeUp;

    [SerializeField]
    Vector2 colliderOffsetDown, colliderOffsetUp;

    [SerializeField]
    bool isDown = false;

    [SerializeField]
    bool hasPopped = true;

    private void Update() {
        if (isDown && hasPopped) {
            StartCoroutine(PopTrampoline());
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Player") && !isDown && hasPopped) {
            Vector2 playerPos = other.transform.position;

            if (playerPos.y <= transform.position.y + 0.5f) {
                isDown = true;
                other.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpBoost);
            }
        }
    }

    private IEnumerator PopTrampoline() {
        hasPopped = false;

        GetComponent<SpriteRenderer>().sprite = downSprite;
        GetComponent<BoxCollider2D>().size = colliderSizeDown;
        GetComponent<BoxCollider2D>().offset = colliderOffsetDown;
        
        yield return new WaitForSeconds(popDelay);

        GetComponent<SpriteRenderer>().sprite = upSprite;
        GetComponent<BoxCollider2D>().size = colliderSizeUp;
        GetComponent<BoxCollider2D>().offset = colliderOffsetUp;

        isDown = false;
        hasPopped = true;
    }
}
