using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterVivTest : MonoBehaviour
{
    private float FlightDurationInSeconds = 2;
    private SpawnVivTest _currentSpawn;
    private Camera _mainCamera;
    private bool _isShot;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    public void ChangeCurrentSpawn(SpawnVivTest NewSpawn)
    {
        _currentSpawn = NewSpawn;
        _isShot = false;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_isShot)
                return;

            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                ShootWithVelocity(hit.point);
            }
        }
    }

    private void ShootWithVelocity(Vector3 TargetPosition)
    {
        _currentSpawn.MoveWithVelocity((TargetPosition - _currentSpawn.transform.position)/FlightDurationInSeconds);
        _isShot = true;
    }
}
