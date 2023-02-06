using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InteractRayCast : MonoBehaviour
{
    // Start is called before the first frame update
    private LayerMask intermask;
    private bool eventcheck = false;
    private GameObject hitObj;
    void Start()
    {
        intermask = LayerMask.GetMask("Interact");
    }

    // Update is called once per frame
    void Update()
    {
        InteractCast();
       // Debug.Log(eventcheck);
    }
    void InteractCast()
    {
        RaycastHit HitHolder = new RaycastHit();

        bool HitsSomething = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out HitHolder,20,intermask);
        eventcheck = HitsSomething;
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward)*20, Color.red);
        if (HitsSomething)
        {
           // Debug.Log(HitsSomething);
             hitObj = HitHolder.transform.gameObject;
            
        
        }
    }
    void OnEInteract()
    {
        Debug.Log("heheha");
        if (eventcheck)

        {
            hitObj.GetComponent<PickUp>().Interact();
//
        }
    }
}
 