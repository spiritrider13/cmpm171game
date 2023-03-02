using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerVivTest : MonoBehaviour
{
    [SerializeField]
    private SpawnVivTest SpawnPrefab;

    [SerializeField]
    private float SpawnDurationInSeconds = 1;

    private ShooterVivTest _spawnShooter;

    public float inventory = 5;

    private bool shot = false;

    private void Start()
    {
        _spawnShooter = GetComponent<ShooterVivTest>();
        NewSpawn();
    }

    private void Update()
    {
        /*if(inventory > 0)
        {
            if (shot)
            {
                inventory -= 1;
                NewSpawn();
                shot = false;
            }
        }*/
    }

    public void NewSpawn()
    {
        _spawnShooter.ChangeCurrentSpawn(Instantiate(SpawnPrefab.gameObject, transform.position, transform.rotation).GetComponent<SpawnVivTest>());
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<SpawnVivTest>())
        {
            //shot = true;
            Invoke(nameof(NewSpawn), SpawnDurationInSeconds);
        }
    }
}
