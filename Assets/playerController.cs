using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;
    public AudioSource register;

    public AudioSource eating;

    public NavMeshAgent agent;

    [SerializeField]
    public GameObject money;

    [SerializeField]
    public MoneyMoneyMoney cashScore;

    [SerializeField]
    public GameObject player;

    public toggleTag seatTag;
    float timer = 0;

    Vector3 moneyPosition; 

    [SerializeField]
    public Transform exit;

    public bool customerFed = false;
    public bool customerPaid = false;
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        exit = GameObject.FindWithTag("Exit").transform;
        player = GameObject.FindWithTag("Player");
        cashScore = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
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

                var clone = Instantiate(money, transform.position + new Vector3(3f, -2f, 0f), transform.rotation); 
                if (Vector3.Distance(player.transform.position, clone.transform.position) < 12)
                {
                    register.Play();
                    print("picked Up");
                    cashScore.addScore(1);
                    //pickedUpMoney = true;
                    //gameObject.tag = "Seats";
                }
                //spawn $$$ on table
                //tell table that customer has paid (bool)
                //table spawns money
                //player can pick up money
                //seat is empty again for customer
                print("money");
                customerPaid = true;
                //seatTag.Empty();
                /*moneyPosition = transform.position + new Vector3(0f, 10f, 0f);
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
                }*/

                //exit building
                agent.SetDestination(exit.position);
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
        if (collision.gameObject.tag == "Dish" && !customerFed)
        {
            print("hit!");

            customerFed = true;
            eating.Play();
            Destroy(collision.gameObject);
        }

    }
}
