using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{
    public playerController target;
    public GameObject money;
    public playerController customer;
    public GameObject player;
    public MoneyMoneyMoney cash;
    public GameObject seat;
    public bool pickedUpMoney = false;
    float timer;
    bool startPickup = false;
    // Start is called before the first frame update
    void Start()
    {
    //target = gameObject;
    player = GameObject.FindWithTag("Player");
        cash = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if(GameObject.FindWithTag("Customer") != null)
        {
            customer = GameObject.FindWithTag("Customer").GetComponent<playerController>();
            if (customer.customerPaid)
            {
                print("money active");
                money.SetActive(true);
                customer.customerPaid = false;
                //pickedUpMoney = false;
                startPickup = true;
                customer.customerFed = false;

            }
        }*/

        /*if ()
        {
            print("customer fed");
            Instantiate(money, transform.position + new Vector3 (0f, 3f, 0f), transform.rotation);
            /*cash.addScore(1);
            //pickedUpMoney = true;
            startPickup = false;
            gameObject.tag = "Seats";
            money.SetActive(false);
        }*/
        if (startPickup)
        {
          //  print("start");
            if (Vector3.Distance(player.transform.position, money.transform.position) < 12)
            {
                print("picked Up");
                cash.addScore(1);
                //pickedUpMoney = true;
                startPickup = false;
                gameObject.tag = "Seats";
                money.SetActive(false); 
            }
        }
    }
}
