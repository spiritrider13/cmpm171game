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

    public GameObject money;

    float timer = 0;

    Vector3 moneyPosition; 

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
        //timer 3 seconds for animation
        if (customerFed)
        {
            //animation
            if (timer < 3)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;

                //spawn $$$ on table
                //tell table that customer has paid (bool)
                //table spawns money
                //player can pick up money
                //seat is empty again for customer
                print("money");
                moneyPosition = transform.position + new Vector3(0f, 10f, 0f);
                var temp = Instantiate(money, moneyPosition, transform.rotation);
                {
                    if (timer < 1)
                    {
                        timer += Time.deltaTime;
                    }
                    else
                    {
                        timer = 0;
                        Destroy(temp.gameObject);
                    }
                }

                //exit building
                agent.SetDestination(exit.position);
                print(Vector3.Distance(gameObject.transform.position, exit.position));
                if(Vector3.Distance(gameObject.transform.position, exit.position) < 15)
                {
                    Destroy(gameObject);
                }
                //customerFed = false;
            }

        }
        //if(transform.position.Distance())
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dish")
        {
            print("hit!");

            customerFed = true;

        }

    }
}
