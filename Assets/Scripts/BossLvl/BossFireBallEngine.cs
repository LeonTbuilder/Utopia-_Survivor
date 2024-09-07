using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBallEngine : MonoBehaviour
{

    public int damage;
    public float shotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        TargetLock();
    }

    private void TargetLock()
    {
        transform.position += transform.forward * shotSpeed * Time.deltaTime;
        Destroy(gameObject, 4f);
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
