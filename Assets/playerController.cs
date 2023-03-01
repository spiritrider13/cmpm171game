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

    float timer = 0;

    [SerializeField]
    public Transform exit;

    public bool customerFed = false;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        exit = GameObject.FindWithTag("Exit").transform;
        //exit = FindTagOfType<Exit>();
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dish")
        {
            print("hit!");

            //timer 3 seconds for animation
            if(timer < 3)
            {
                timer += Time.deltaTime;
            } else
            {
                timer = 0;
                //animation

                //spawn $$$ on table

                //exit building
                agent.SetDestination(exit.position);
            }

        }

    }
}
