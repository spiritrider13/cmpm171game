using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFed : MonoBehaviour
{

    public MoneyMoneyMoney money;
    public CustomerManager seats;


    public float speed;
    public Transform[] moveSpots;
    public Transform[] route2;
    private int randomSpot;
    public int pickRoute;

    // Start is called before the first frame update
    void Start()
    {
        //pickRoute = Random.Range(1, 2);
        pickRoute = 2;
        money = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
        seats = GameObject.FindGameObjectWithTag("Seats").GetComponent<CustomerManager>();
        randomSpot = 0;
    }

    private void Update()
    {
        if(pickRoute == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
            {
                if (randomSpot == 0 || randomSpot >= 2)
                {
                    if (randomSpot != moveSpots.Length - 1)
                    {
                        randomSpot += 1;
                    }
                }
                else
                {
                    seats.seatCustomers();
                }
                print("route 1 @ spot");
            }

        } else if(pickRoute == 2)
        {
            transform.position = Vector3.MoveTowards(transform.position, route2[randomSpot].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, route2[randomSpot].position) < 0.2f)
            {
                if (randomSpot <= 1 || randomSpot >= 3)
                {
                    if (randomSpot != route2.Length - 1)
                    {
                        randomSpot += 1;
                    }
                }
                else
                {
                    seats.seatCustomers();
                }
                print("route 2 @ spot");
            }
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            Destroy(collision.gameObject);
            money.addScore(4);
            if(pickRoute == 1)
            {
                randomSpot = 2;
            } else if(pickRoute == 2)
            {
                randomSpot = 3;
            }
        }

    }
}
