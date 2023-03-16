using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public MoneySpawner spawner;

    public playerController customer;
    // Start is called before the first frame update
    void Start()
    {
    }


    // Update is called once per frame
    void Update()
    {
        
        /*spawner = GameObject.FindWithTag("Untagged").GetComponent<MoneySpawner>();
        if (GameObject.FindWithTag("Customer") != null)
        {
            customer = GameObject.FindWithTag("Customer").GetComponent<playerController>();
            if (spawner.pickedUpMoney && !customer.customerPaid)
            {
                Destroy(gameObject);
            }
        }
            /*else if (!pickedUpMoney && temp != null)
        {
            
        }*/


    }



}
