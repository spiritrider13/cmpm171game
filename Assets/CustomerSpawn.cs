using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private CustomerSpawner _spawner;

    //private SeatQueue _line;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spawner = GetComponent<CustomerSpawner>();

        //if (_spawner.AssignSeat())
        {

        }

        //_line.AddToWait(gameObject);
    }
}
