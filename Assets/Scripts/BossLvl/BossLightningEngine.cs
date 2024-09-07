using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossLightningEngine : MonoBehaviour
{
    public int damage;
    private int hitCount;
    private List<BossEnemyEngine> hitEnemies = new List<BossEnemyEngine>();
    private bool hasHitTargetEnemy = false;
    public float lightningSpeed;
    private Transform currentTarget;

    void Start()
    {
        hitCount = 2;
        currentTarget = BossPlayerEngine.instance.GetClosestEnemy();
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
            currentTarget.GetComponent<BossEnemyEngine>()?.ReduceHealthByLightning();
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
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Boss")) && !hitEnemies.Contains(other.gameObject.GetComponent<BossEnemyEngine>()) && other.gameObject.GetComponent<BossEnemyEngine>().hP > 0)
        {
            hitCount--;
            currentTarget = other.transform;
            other.gameObject.GetComponent<BossEnemyEngine>().hP -= damage;
            hitEnemies.Add(other.gameObject.GetComponent<BossEnemyEngine>());
            hasHitTargetEnemy = other.gameObject == currentTarget.gameObject;
            print(hitCount);
        }
    }

    private BossEnemyEngine GetNextClosestEnemy(Transform fromEnemy)
    {
        float minDistance = Mathf.Infinity;
        BossEnemyEngine closestEnemy = null;

        foreach (BossEnemyEngine enemy in FindObjectsOfType<BossEnemyEngine>())
        {
            if (enemy != fromEnemy.GetComponent<BossEnemyEngine>())
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
