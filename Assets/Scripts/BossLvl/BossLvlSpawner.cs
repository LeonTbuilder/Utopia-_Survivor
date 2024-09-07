using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossLvlSpawner : MonoBehaviour
{
    public GameOverManager gameManager;
    public static BossLvlSpawner instance;
    public static List<GameObject> enemies = new List<GameObject>();
    public GameObject[] enemyPrefabs;
    public GameObject bossPrefab;
    public float spawnRadius;

    public int enemyCap;
    public int enemyCount;

    private GameObject bossInstance;

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
        SpawnBoss();
        StartCoroutine(SpawnEnemyRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = enemies.Count;
    }

    private void SpawnBoss()
    {
        Vector2 randomPointOnCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomPointOnCircle.x, 0, randomPointOnCircle.y) + transform.position;
        bossInstance = Instantiate(bossPrefab, spawnPos, Quaternion.identity);
        enemies.Add(bossInstance);
    }

    private void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector2 randomPointOnCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomPointOnCircle.x, 0, randomPointOnCircle.y) + transform.position;
        enemies.Add(Instantiate(enemyPrefab, spawnPos, Quaternion.identity));
    }

    private IEnumerator SpawnEnemyRoutine()
    {
        while (bossInstance != null) 
        {
            if (enemyCount < enemyCap)
            {
                SpawnEnemy(enemyPrefabs[0]);
                SpawnEnemy(enemyPrefabs[1]);
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public void DeadBoss(GameObject enemy)
    {
        if (enemy.tag == "Boss")
        {
            gameManager.WinGame();
        }
    }
}
