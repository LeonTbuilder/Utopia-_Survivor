using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossWaterEngine : MonoBehaviour
{
    public float waterSpeed;
    public int damage;

    PlayerEngine playerEngine;
    Transform newTarget;


    Vector3 velocity;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
        newTarget = BossPlayerEngine.instance.GetClosestEnemy();

        if (newTarget != null)
        {
            velocity = (newTarget.position - transform.position).normalized * waterSpeed * Time.deltaTime;
        }
    }

    // Update is called once per frame
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
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Boss")))
        {
            other.gameObject.GetComponent<BossEnemyEngine>().hP -= damage;
            Destroy(gameObject);
        }
    }

}
