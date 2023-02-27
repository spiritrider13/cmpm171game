using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using GameCreator.Runtime.Variables;

public class SeatQueue : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Lazercomponent;
    public LocalNameVariables SeatGrabbed;
    private List<GameObject> WaitingCustomers = new List<GameObject>();

    void Start()
    {
        //WaitingCustomers = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void AddToWait(GameObject spawnedcustomer)
    {
        print("new customer!");
        WaitingCustomers.Add(spawnedcustomer);
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
