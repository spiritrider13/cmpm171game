using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVivTest : MonoBehaviour
{
    [SerializeField]
    private SpawnVivTest SpawnPrefab;

    [SerializeField]
    private float SpawnDurationInSeconds = 2;

    private ShooterVivTest _spawnShooter;

    private void Start()
    {
        _spawnShooter = GetComponent<ShooterVivTest>();
        NewSpawn();
    }

    public void NewSpawn()
    {
        _spawnShooter.ChangeCurrentSpawn(Instantiate(SpawnPrefab.gameObject, transform.position, transform.rotation).GetComponent<SpawnVivTest>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SpawnVivTest>())
        {
            Invoke(nameof(NewSpawn), SpawnDurationInSeconds);
        }
    }
}
