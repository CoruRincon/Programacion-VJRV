using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float minLimit, maxLimit;

    float cameraPosX;
    float cameraPosY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosX = Mathf.Clamp(player.position.x, minLimit, maxLimit);

        transform.position = new Vector3 (cameraPosX, transform.position.y, transform.position.z);
    }
}
