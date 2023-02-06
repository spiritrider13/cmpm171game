using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetServed : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnCollisionEnter(Collision incoming)
    {
        if (incoming.gameObject.tag == "Dish")
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red);
        }
    }

}
