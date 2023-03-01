using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;

    public NavMeshAgent agent;

    private SeatingLaser laser;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (laser.HitsSomething)
        {
            print("move");
                agent.SetDestination(laser.transform.position);
        }*/
    }
}
