using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomerManager : MonoBehaviour
{
    public int customersSeated;

    public Text customers;


    [ContextMenu("Seated Customers")]
    public void seatCustomers()
    {
        customersSeated += 1;
        customers.text = customersSeated + "/2";
    }

}
