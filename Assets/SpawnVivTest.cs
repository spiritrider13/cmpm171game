using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVivTest : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void MoveWithVelocity(Vector3 Velocity)
    {
        _rigidbody.velocity = Velocity;
    }
}
