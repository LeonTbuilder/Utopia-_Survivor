using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossCamEngine : MonoBehaviour
{

    public int Ypos;
    public int ZPos;
    public BossPlayerEngine target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindObjectOfType<BossPlayerEngine>();
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
            gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + Ypos, target.transform.position.z - ZPos);
    }
}
