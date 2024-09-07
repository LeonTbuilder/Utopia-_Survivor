using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterEngine : MonoBehaviour
{
    public float waterSpeed;
    public int damage;

    private Transform newTarget;

    void Start()
    {
        damage = 1;
        newTarget = PlayerEngine.instance.FindClosestEnemy();  // Updated to use FindClosestEnemy

        if (newTarget != null)
        {
            Vector3 velocity = (newTarget.position - transform.position).normalized * waterSpeed * Time.deltaTime;
        }
    }

    void Update()
    {
        if (newTarget != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, newTarget.position, waterSpeed * Time.deltaTime);
            Destroy(gameObject, 4f);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Boss"))
        {
            other.gameObject.GetComponent<EnemyEngine>().hP -= damage;  // Updated to use health
            Destroy(gameObject);
        }
    }
}
