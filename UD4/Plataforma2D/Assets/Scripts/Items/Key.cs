using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField]
    private GameObject door;

    public void OpenDoor() {
        door.SetActive(false);
    }
}
