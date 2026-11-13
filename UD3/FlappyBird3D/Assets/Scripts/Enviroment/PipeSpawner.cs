using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class PipeSpawner : MonoBehaviour
{

    public static PipeSpawner Instance;

    [Header("Spawner Settings")]
    [SerializeField]
    private GameObject pipePrefab;
    [SerializeField]
    private float spawnCoolDown;
    [SerializeField]
    private int maxPipes;
    [SerializeField]
    private Transform spawnPos;

    [Header("Pipes Settings")]
    [SerializeField]
    private float pipeSpeed;

    [SerializeField]
    private float maxHeight = 2.85f, minHeight = -1.45f;

    [SerializeField]
    private float maxSeparation = 3.0f, minSeparation = 2.0f;

    [SerializeField]
    private Dictionary<GameObject, bool> pipePool;

    void Awake() {
        if(Instance == null) {
            Instance = this;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pipePool = new Dictionary<GameObject, bool>();
        
        /*for (int i = 0; i < maxPipes; i++)
        {
            GameObject thisPipe = Instantiate(pipePrefab, spawnPos.position, Quaternion.identity);
            thisPipe.transform.parent = transform;

            pipePool.Add(thisPipe, true);
        }*/

        StartCoroutine(PipesSpawner());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnPipe() {
        float randomHeight = Random.Range(minHeight, maxHeight);
        float randomSeparation = Random.Range(minSeparation, maxSeparation);

        GameObject thisPipe = Instantiate(pipePrefab, spawnPos.position, Quaternion.identity);
        thisPipe.transform.parent = transform;

        thisPipe.transform.position = spawnPos.position;
        thisPipe.GetComponent<Pipe>().UpdatePipe(-pipeSpeed, randomHeight, randomSeparation);

        pipePool[thisPipe] = false;
    }


    public void ResetPipe(GameObject thisPipe) {
        thisPipe.transform.position = spawnPos.position;
        thisPipe.GetComponent<Pipe>().InitializePipe();
    }

    private IEnumerator PipesSpawner() {
        while(true) {
            yield return new WaitForSeconds(spawnCoolDown);

            /*GameObject firstAvailablePipe = pipePool.FirstOrDefault(x => x.Value  == true).Key;
            
            if(firstAvailablePipe != null) {
                Debug.Log("Spawning Pipe");

                SpawnPipe(firstAvailablePipe);
            }*/

            SpawnPipe();
        }
    }
    
}
