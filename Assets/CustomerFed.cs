using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFed : MonoBehaviour
{

    public MoneyMoneyMoney money;


    public float speed;
    public Transform[] moveSpots;
    private int randomSpot;

    // Start is called before the first frame update
    void Start()
    {
        money = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
        randomSpot = 0;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
        {
            if (randomSpot == 0 || randomSpot >= 2)
            {
                if(randomSpot != moveSpots.Length - 1)
                {
                    randomSpot += 1;
                }
            }
            print("@ spot");
        }
    }


    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 7)
        {
            money.addScore(4);
            randomSpot = 2;
        }

    }
}
