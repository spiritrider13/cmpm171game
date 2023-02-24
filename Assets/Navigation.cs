using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigation : MonoBehaviour
{
    public float speed;
    public Transform[] moveSpots;
    private int randomSpot;

    void Start()
    {
        randomSpot = Random.Range(0, moveSpots.Length);
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) <0.2f)
        {
            if(randomSpot == 0)
            {
                randomSpot += 1;
            }
            /*
            if (gameObject.OnCollisionEnter)
            {

            }
            */
        }
    }
}
