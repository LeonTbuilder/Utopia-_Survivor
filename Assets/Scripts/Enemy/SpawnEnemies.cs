using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    public GameOverManager gameManager;
    public static SpawnEnemies instance;
    public static List<GameObject> enemies = new List<GameObject>();
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public float spawnRadius;

    public int enemyCap;
    public int enemyCount;

    private float spawnTimer;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = enemies.Count;
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 randomPointOnCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomPointOnCircle.x, 0, randomPointOnCircle.y) + transform.position;
        enemies.Add(Instantiate(enemyPrefab, spawnPos, Quaternion.identity));
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            if (spawnTimer < 15f)
            {
                if (enemyCount < enemyCap)
                {
                    SpawnEnemy(enemyPrefabs[0]);
                }
            }
            else if (spawnTimer < 30f)
            {
                if (enemyCount < enemyCap)
                {
                    SpawnEnemy(enemyPrefabs[1]);
                    SpawnEnemy(enemyPrefabs[0]);
                }
            }
            else if (spawnTimer < 60f)
            {
                if (enemyCount < enemyCap)
                {
                    SpawnEnemy(enemyPrefabs[2]);
                    SpawnEnemy(enemyPrefabs[1]);
                    SpawnEnemy(enemyPrefabs[0]);
                }
            }
            else if (spawnTimer < 90f)
            {
                if (enemyCount < enemyCap)
                {
                    SpawnEnemy(enemyPrefabs[3]);
                    SpawnEnemy(enemyPrefabs[2]);
                    SpawnEnemy(enemyPrefabs[1]);
                    SpawnEnemy(enemyPrefabs[0]);
                }
            }
            else
            {
                foreach (var enemy in enemies)
                {
                    Destroy(enemy);
                }
                enemies.Clear();
                SpawnEnemy(bossPrefab);
                yield break;
            }

            spawnTimer += 1f;
            yield return new WaitForSeconds(1f);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        if (enemy.tag == "Boss")
        {
            gameManager.WinGame();
        }
    }
}
