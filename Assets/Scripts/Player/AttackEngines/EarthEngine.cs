using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthEngine : MonoBehaviour
{
    public int damage;
    public GameObject earthPrefab;
    public PlayerEngine playerEngine; 

    void Start()
    {
        damage = 1;
        playerEngine = GameObject.FindObjectOfType<PlayerEngine>();
    }

    void Update()
    {
        Destroy(gameObject, 4f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") || (other.gameObject.CompareTag("Boss")))
        {
            other.gameObject.GetComponent<EnemyEngine>().hP -= damage;
        }
    }

}
