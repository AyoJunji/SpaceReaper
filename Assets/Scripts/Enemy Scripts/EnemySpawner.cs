using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject alienGhost;
    public GameObject alienExploder;
    public GameObject alienSpitter;

    public float ghostTimer = 2f;
    public float exploderTimer = 4f;
    public float spitterTimer = 6f;

    public float spawnTimer = 12f;

    void Start()
    {
        StartCoroutine(SpawnEnemy(ghostTimer, alienGhost));
        StartCoroutine(SpawnEnemy(exploderTimer, alienExploder));
        StartCoroutine(SpawnEnemy(spitterTimer, alienSpitter));
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy)
    {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);

        if (spawnTimer >= 0)
        {
            StartCoroutine(SpawnEnemy(interval, enemy));
        }
    }
}