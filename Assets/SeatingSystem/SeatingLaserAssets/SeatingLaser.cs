using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Runtime.Variables;
using UnityEngine.AI;


public class SeatingLaser : MonoBehaviour
{
    // Start is called before the first frame update
   
    private bool eventcheck = false;
    public LineRenderer Laserbeam;
    private LocalNameVariables SeatShop;
    public GameObject hitObj;
    public CustomerSpawner list;
    //public toggleTag booth;
    //public returnPosition seat;


    //public NavMeshAgent agent;

    void Start()
    {
        SeatShop = GetComponent<LocalNameVariables>();
        Laserbeam = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    public void Update()
    {
        //LaserCast();
        // Debug.Log(eventcheck);
    }
   public void LaserCast()
    {

        Laserbeam.enabled = true;
       
        //  Laserbeam.enabled = false;

        RaycastHit HitHolder = new RaycastHit();

        bool HitsSomething = Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out HitHolder);
        eventcheck = HitsSomething;
        //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 4000, Color.red);
        if (HitsSomething)
        {
            // Laserbeam.enabled = true;
            hitObj = HitHolder.transform.gameObject;
            if (hitObj.tag == "Seats")
            {
              //  Laserbeam.startColor = Color.green;
                Laserbeam.material.color =Color.green;
            }
            else
            {
                Laserbeam.material.color= Color.red;
            }
            Vector3[] BeamLine = new Vector3[] { transform.position, HitHolder.point };
            Laserbeam.SetPositions(BeamLine);
             Debug.Log(transform.position);
           // SeatAssign();
            //list.AssignSeat(transform.position);
            //agent.SetDestination(transform.position);
         //   hitObj = HitHolder.transform.gameObject;


        }
        else
        {
            Vector3[] BeamLine = new Vector3[] { transform.position, transform.position+(transform.forward*500)};
            Laserbeam.SetPositions(BeamLine);
            Laserbeam.material.color = Color.red;
         
        }
    }

   public void SeatAssign()
         
    {
      
        Laserbeam.enabled = false;

        if (hitObj && hitObj.tag == "Seats"  )
        {
            //Set seat as taken
            //  hitObj.invoke()
           Debug.Log("SEAT SELECTED");
            hitObj.tag = "Untagged";
            list.AssignSeat(hitObj.GetComponent<toggleTag>().returnMoveSpotPosition());
  
                SeatShop.Set("SeatObj", hitObj);
            
        }
      

    }
   
}

