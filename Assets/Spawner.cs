using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
a general purpose gameobject that contains functions to spawn things in. dont worry about triggers. we need a function that can spawn x things in an area x units wide. we will use the spawners in conjunction with the timer system to spawn customers over time. We will use the unity event system, so check if the event system allows parameters, or if we need to store things like item amount in the spawner script itself. If we have to store it in the spawner, we will need functions that can be called to alter the behavior of the spawner. (Spawn amount, area size, time delay between spawns etc)
Essentially, think of these spawners as radio stations waiting for a signal.
*/

//what we want is when triggered, an object will spawn

public class spawn : MonoBehaviour
{
    public GameObject objectToSpawn;
    
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.tag == 'Player')
        {
            Debug.Log("triggered spawn");
            Instantiate(objectToSpawn);
        }
    }
}
