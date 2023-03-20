using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class playerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Camera cam;

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
    private LayerMask LayerSeated;
    private LayerMask Layerboothback;
    private LayerMask Layerboothseat;
    private GameObject Assignedbooth;
    private Vector3 Neutralizer =new Vector3(1, 0, 1);
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        exit = GameObject.FindWithTag("Exit").transform;
        player = GameObject.FindWithTag("Player");
        cashScore = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
         LayerSeated = LayerMask.NameToLayer("SeatedCustomers"); 
        Layerboothback = LayerMask.NameToLayer("booth seats");
        Layerboothseat= LayerMask.NameToLayer("booth backing");
        BroadcastMessage("StartTimer");
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

                //var clone = Instantiate(money, transform.position + new Vector3(3f, 0f, 0f), transform.rotation); 
              /*  if (Vector3.Distance(player.transform.position, clone.transform.position) < 12)
                {
                    print("picked Up");
                    cashScore.addScore(1);
                    //pickedUpMoney = true;
                    gameObject.tag = "Seats";
                }
              */
                //spawn $$$ on table
                //tell table that customer has paid (bool)
                //table spawns money
                //player can pick up money
                //seat is empty again for customer
             //   print("money");
               // customerPaid = true;
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
                ExitDiner();
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

           Assignedbooth.transform.GetChild(2).transform.GetComponent<MoodMoneySpawn>().MoodBasedTip(GetComponentInChildren<MoodFrame>().tiptype);
            Destroy(collision.gameObject);

        }
        if (collision.gameObject.layer == Layerboothseat || collision.gameObject.layer == Layerboothback)
        {

            Transform IsForMe;
            if (collision.gameObject.transform.parent != null)
            {
                IsForMe = collision.gameObject.transform.parent;
            }
            else IsForMe = collision.gameObject.transform;

           Debug.Log(Vector3.Dot(IsForMe.GetChild(1).position,Neutralizer)+"waah"+Vector3.Dot(agent.destination,Neutralizer));
            if (Vector3.Dot(IsForMe.GetChild(1).position,Neutralizer) == Vector3.Dot(agent.destination,Neutralizer))
            {

                gameObject.layer = LayerSeated;
                Assignedbooth = IsForMe.gameObject;
                BroadcastMessage("StartTimer");
              Debug.Log("HELLO");

            }
            //print("why god");
            else {
              //  print("sussy");
              //  Debug.Log(IsForMe.GetChild(1).position + "huh" + agent.destination);
                return;
            }

        }

    }


    public void OnCollisionExit(Collision collision)
    {
       
        if(collision.gameObject.tag == "booth seats" && gameObject.layer == LayerSeated)
        {
            gameObject.layer = LayerMask.NameToLayer("Customers");
           // 
        }
    }
   
    public void ExitDiner()
    {
        agent.SetDestination(exit.position);
        if (Vector3.Distance(gameObject.transform.position, exit.position) < 15)
        {
            Destroy(gameObject);
        }
    }
}
