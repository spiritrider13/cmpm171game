using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour
{

    public GameObject money;
    public playerController customer;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.FindWithTag("Customer") != null)
        {
            customer = GameObject.FindWithTag("Customer").GetComponent<playerController>();
            if (customer.customerPaid)
            {
                Instantiate(money, transform.position + new Vector3 (0f,1f,0f), transform.rotation);
                customer.customerPaid = false;
            }

        }
    }
}
