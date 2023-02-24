using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerFed : MonoBehaviour
{

    public MoneyMoneyMoney money;
    // Start is called before the first frame update
    void Start()
    {
        money = GameObject.FindGameObjectWithTag("Money").GetComponent<MoneyMoneyMoney>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == 7)
            money.addScore(4);
    }
}
