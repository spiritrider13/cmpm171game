using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public GameObject player;
    public MoneyMoneyMoney money;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        money = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(player.transform.position, transform.position)< 5)
        {
            money.addScore(1);
            Destroy(gameObject);
        }
    }
}
