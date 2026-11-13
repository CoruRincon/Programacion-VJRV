using UnityEngine;

public class Pipe : MonoBehaviour {
    [Header("Pipe Components")]
    [SerializeField]
    private GameObject pipeTop;

    [SerializeField]
    private GameObject pipeBottom;

    [Header("Pipe Settings")]
    [SerializeField]
    private float pipeSpeed;

    [SerializeField]
    private float crusherSpeed;

    [SerializeField]
    private float pipeHeight;

    [SerializeField]
    private float pipeSpace;

    private Vector3 middlePipe;

    private float currentSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        InitializePipe();
    }

    // Update is called once per frame
    void Update() {
        InitializePipe();

        Vector3 currentPos = transform.position;
        float movement = currentSpeed * Time.deltaTime;

        Vector3 newPos = currentPos + Vector3.right * movement;

        transform.position = newPos;
    }

    public void InitializePipe() {
        Vector3 pipeTopPos = pipeTop.transform.localPosition;
        Vector3 pipeBotPos = pipeBottom.transform.localPosition;

        Vector3 pipeGlobalPos = transform.position;
        transform.position = new Vector3(pipeGlobalPos.x, pipeHeight, pipeGlobalPos.z);


        middlePipe = (pipeTopPos + pipeBotPos) / 2;

        float pipeSpacePos = middlePipe.y + (pipeSpace / 2) + (pipeTop.transform.lossyScale.y + pipeTop.transform.GetChild(0).lossyScale.y);

        Vector3 pipeTopY = new Vector3(pipeTopPos.x, pipeSpacePos, pipeTopPos.z);
        Vector3 pipeBotY = new Vector3(pipeBotPos.x, -pipeSpacePos, pipeBotPos.z);

        pipeTop.transform.localPosition = pipeTopY;
        pipeBottom.transform.localPosition = pipeBotY;

        currentSpeed = pipeSpeed;
    }

    public void UpdatePipe(float thisSpeed, float thisHeight, float thisSpace) {
        pipeSpeed = thisSpeed;
        pipeHeight = thisHeight;
        pipeSpace = thisSpace;
    }

    public void OnBecameInvisible() {
        Destroy(gameObject, 5.0f);
    }
}
