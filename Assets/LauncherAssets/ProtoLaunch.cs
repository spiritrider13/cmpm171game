using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtoLaunch : MonoBehaviour
{
    public GameObject Ammo;
    public float LaunchVel = 700f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        
    }


    //Will need if in inventory check
    //Currently when LMB is pressed, it will fire no matter what. 
    //However, Launching an item from inventory means 
    //Telling the launcher if it can launch
    // if it can,  tell it what to launch

    //Either have the launcher contain the inventory or have the player contain the inventory,
    // and make an additional event call which the launcher waits for instead of the LMB input event
    //
   public void OnLaunch()
    {

        GameObject NewProjectile = Instantiate(Ammo, transform.position, transform.rotation);
        NewProjectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3( 0, LaunchVel, 0));
    }

    //public void OnSwitch


    //Replace update with event system stuff
}
