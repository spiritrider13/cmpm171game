using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVivTest : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private SpawnerVivTest _spawner;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _spawner = GetComponent<SpawnerVivTest>();
    }



    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);

    }
    public void MoveWithVelocity(Vector3 Velocity)
    {
        _rigidbody.velocity = Velocity;
    }
}
