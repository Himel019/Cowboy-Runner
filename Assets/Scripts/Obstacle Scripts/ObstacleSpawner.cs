using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public static ObstacleSpawner instance;

    [SerializeField]
    private GameObject[] obstacles;

    private List<GameObject> obstaclesForSpawning = new List<GameObject>();

    [SerializeField]
    private float minSpawningSpeed = 0.1f;
    [SerializeField]
    private float maxSpawningSpeed = 4f;

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {
        MakeInstance();
        InitializeObstacles();
    }

    // Start is called before the first frame update
    void Start()
    {
        Shuffle();
        StartCoroutine(SpawnRandomObstacle());
    }

    private void MakeInstance() {
        if(instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    private void InitializeObstacles() {
        int index = 0;

        for(int i = 0; i < obstacles.Length * 3; i++) {
            GameObject obj = Instantiate(obstacles[index], transform.position, Quaternion.identity) as GameObject;
            obstaclesForSpawning.Add(obj);
            obstaclesForSpawning[i].SetActive(false);
            index++;

            if(index == obstacles.Length) {
                index = 0;
            }
        }
    }

    private void Shuffle() {
        for(int i = 0; i < obstaclesForSpawning.Count; i++) {
            GameObject temp = obstaclesForSpawning[i];

            int random = Random.Range(i, obstaclesForSpawning.Count);

            obstaclesForSpawning[i] = obstaclesForSpawning[random];
            obstaclesForSpawning[random] = temp;
        }
    }

    public void SetNewObstacleSpeed(float addSpeed) {
        for(int i = 0; i < obstaclesForSpawning.Count; i++) {
            obstaclesForSpawning[i].GetComponent<ObstacleMovement>().SetSpeed(addSpeed);
        }
    }

    private IEnumerator SpawnRandomObstacle() {
        yield return new WaitForSeconds(Random.Range(minSpawningSpeed, maxSpawningSpeed));

        int index = Random.Range(0, obstaclesForSpawning.Count);

        while(true) {
            if(!obstaclesForSpawning[index].activeInHierarchy) {
                obstaclesForSpawning[index].SetActive(true);
                obstaclesForSpawning[index].transform.position = transform.position;
                break;
            } else {
                index = Random.Range(0, obstaclesForSpawning.Count);
            }
        }

        StartCoroutine(SpawnRandomObstacle());
    }

    public float GetSpawningSpeed()  {
        return maxSpawningSpeed;
    }

    public void SetSpawningTime(float speed) {
        if(maxSpawningSpeed > 1.5f) {
            maxSpawningSpeed -= speed;
        }
    }
}
