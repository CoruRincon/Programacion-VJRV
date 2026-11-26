using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField]
    private Transform player;

    [SerializeField]
    private float minLimitX, maxLimitX;

    [SerializeField]
    private float minLimitY, maxLimitY;

    float cameraPosX;
    float cameraPosY;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosX = Mathf.Clamp(player.position.x, minLimitX, maxLimitX);
        cameraPosY = Mathf.Clamp(player.position.y, minLimitY, maxLimitY);

        transform.position = new Vector3 (cameraPosX, cameraPosY, transform.position.z);
    }
}
