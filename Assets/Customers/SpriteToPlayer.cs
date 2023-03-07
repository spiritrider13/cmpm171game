using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteToPlayer : MonoBehaviour
{
    public Transform target;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {   
        Vector3 modifiedTarget = target.position;
     //   Vector3 modifiedTarget = gameObject.transform.position;
        modifiedTarget.y = transform.position.y;
        transform.LookAt(modifiedTarget);
    }
}
