using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEarthEngine : MonoBehaviour
{
    public int damage;
    public GameObject earthPrefab;
    public BossPlayerEngine playerEngine;

    void Start()
    {
        damage = 1;
        playerEngine = GameObject.FindObjectOfType<BossPlayerEngine>();
    }

    void Update()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Boss")))
        {
            other.gameObject.GetComponent<BossEnemyEngine>().hP -= damage;
        }
    }

}
