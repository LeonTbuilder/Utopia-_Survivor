using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirSphereEngine : MonoBehaviour
{
    public int damage;
    public GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        damage = 1;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;

    }

}
