using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningEngine : MonoBehaviour
{
    public int damage;
    private int hitCount;
    private List<EnemyEngine> hitEnemies = new List<EnemyEngine>();
    private bool hasHitTargetEnemy = false;
    public float lightningSpeed;
    private Transform currentTarget;

    void Start()
    {
        hitCount = 2;
        currentTarget = PlayerEngine.instance.FindClosestEnemy();  // Updated to use FindClosestEnemy
        damage = 1;
    }

    void Update()
    {
        if (currentTarget != null && !hasHitTargetEnemy)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget.position, lightningSpeed * Time.deltaTime);
        }

        if (currentTarget != null && hasHitTargetEnemy && hitCount > 0)
        {
            currentTarget.GetComponent<EnemyEngine>()?.ReduceHealthByLightning();
            currentTarget = GetNextClosestEnemy(currentTarget)?.transform;
            hasHitTargetEnemy = false;
        }

        else
        {
            Destroy(gameObject, 0.5f);
        }

        if (hitCount <= 0 || currentTarget == null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Boss")) && !hitEnemies.Contains(other.gameObject.GetComponent<EnemyEngine>()) && other.gameObject.GetComponent<EnemyEngine>().hP > 0)
        {
            hitCount--;
            currentTarget = other.transform;
            other.gameObject.GetComponent<EnemyEngine>().hP -= damage;
            hitEnemies.Add(other.gameObject.GetComponent<EnemyEngine>());
            hasHitTargetEnemy = other.gameObject == currentTarget.gameObject;
            print(hitCount);
        }
    }

    private EnemyEngine GetNextClosestEnemy(Transform fromEnemy)
    {
        float minDistance = Mathf.Infinity;
        EnemyEngine closestEnemy = null;

        foreach (EnemyEngine enemy in FindObjectsOfType<EnemyEngine>())
        {
            if (enemy != fromEnemy.GetComponent<EnemyEngine>())
            {
                float distance = Vector3.Distance(fromEnemy.position, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }
}
