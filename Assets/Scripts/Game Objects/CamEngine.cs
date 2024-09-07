using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CamEngine : MonoBehaviour
{

    public int Ypos;
    public int ZPos;
    public PlayerEngine target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<PlayerEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        gameObject.transform.position = new Vector3(target.transform.position.x,target.transform.position.y+ Ypos, target.transform.position.z- ZPos);
    }
}
