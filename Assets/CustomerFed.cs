using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFed : MonoBehaviour
{

    public MoneyMoneyMoney money;
    public CustomerManager seats;
    public moveToBooth1 booth1;


    public float speed;
    public Transform[] route1;
    public Transform[] route2;
    public Transform[] enterRestaurant;
    public int randomSpot;
    public int pickRoute;
    public bool leaving = false;

    // Start is called before the first frame update
    void Start()
    {
        //pickRoute = Random.Range(1, 2);
        pickRoute = 0;
        money = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
        seats = GameObject.FindGameObjectWithTag("Seats").GetComponent<CustomerManager>();
        booth1 = GameObject.FindGameObjectWithTag("Booth1").GetComponent<moveToBooth1>();
        randomSpot = 0;
    }

    private void Update()
    {
        if (booth1.seated)
        {
            moveToSeating(1);
        } else if (leaving) {
            leaveSeat(pickRoute);
        } else
        {
            moveToSeating(0);
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            print("hit!");
            print(pickRoute);
            Destroy(collision.gameObject);
            money.addScore(4);
            if(pickRoute == 1)
            {
                booth1.seated = false;
                leaving = true;
            } else if(pickRoute == 2)
            {
                randomSpot = 3;
            }
        }

    }
    public void moveToSeating(int route)
    {
        pickRoute = route;
        if (route == 1)
        {
            transform.position = Vector3.MoveTowards(transform.position, route1[randomSpot].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, route1[randomSpot].position) < 0.2f)
            {
                if (randomSpot == 0 || randomSpot >= 2)
                {
                    if (randomSpot != route1.Length - 1)
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

        }
        else if (route == 2)
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
        else if (route == 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, enterRestaurant[randomSpot].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, enterRestaurant[randomSpot].position) < 0.2f)
            {
                if (randomSpot != enterRestaurant.Length - 1)
                {
                    randomSpot += 1;
                }
                print("entered restaurant");
            }
        }

    }
    private void leaveSeat(int route)
    {
        if (route == 1)
        {
            randomSpot = 0;
            transform.position = Vector3.MoveTowards(transform.position, route1[route1.Length - randomSpot - 1].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, route1[route1.Length - randomSpot - 1].position) < 0.2f)
            {
                if (randomSpot != route1.Length - 1)
                {
                    randomSpot += 1;
                } else
                {
                    print("leaving" + randomSpot);
                    pickRoute = -1;
                }
            }

        } else if(route == -1)
        {
            transform.position = Vector3.MoveTowards(transform.position, enterRestaurant[enterRestaurant.Length - randomSpot - 1].position, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, enterRestaurant[enterRestaurant.Length - randomSpot - 1].position) < 0.2f)
            {
                if (randomSpot != enterRestaurant.Length - 1)
                {
                    print(randomSpot);
                    print(pickRoute);
                    randomSpot += 1;
                } else
                {
                    print("exited restaurant");
                    leaving = false;
                }
            }

        }


    }
}
