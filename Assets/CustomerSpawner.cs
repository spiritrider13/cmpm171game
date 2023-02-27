using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameCreator.Runtime.Variables;

public class CustomerSpawner : MonoBehaviour
{

    public GameObject customer;

    float timer = 0;

    private Vector3 customerSpacing = new Vector3(0f, 0f, -8f);

    //public SeatQueue line;

    private List<GameObject> WaitingCustomers = new List<GameObject>();


    public GameObject Lazercomponent;
    public LocalNameVariables SeatGrabbed;

    private void Start()
    {
        var clone = Instantiate(customer, transform.position, transform.rotation);
        WaitingCustomers.Add(clone);
        //AssignSeat();
    }

    private void Update()
    {
        /*if (timer < 3)
        {
            timer += Time.deltaTime;
        } else
        {
            timer = 0;
            transform.position += customerSpacing;
            var clone = Instantiate(customer, transform.position, transform.rotation);
            WaitingCustomers.Add(clone);
        }*/
    }
    public bool AssignSeat(Vector3 custnavdest)
    {
        if (WaitingCustomers.Count > 0)
        {
            WaitingCustomers[0].GetComponent<NavMeshAgent>().SetDestination(custnavdest);
            WaitingCustomers.RemoveAt(0);
            return true;
        }
        return false;
    }

}
