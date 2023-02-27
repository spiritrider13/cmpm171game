using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveToBooth1 : MonoBehaviour
{


    public CustomerFed customer;
    public bool seated = false;

    // Start is called before the first frame update
    void Start()
    {
        customer = GameObject.FindGameObjectWithTag("Customer").GetComponent<CustomerFed>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print("going to booth 1!");
            //customer.pickRoute = 1;
            customer.randomSpot = 0;
            seated = true;
        }
    }
}
