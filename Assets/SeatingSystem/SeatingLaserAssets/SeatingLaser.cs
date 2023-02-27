using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameCreator.Runtime.Variables;


public class SeatingLaser : MonoBehaviour
{
    // Start is called before the first frame update
   
    private bool eventcheck = false;
    private LineRenderer Laserbeam;
    private LocalNameVariables SeatShop;
    public GameObject hitObj;
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
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 4000, Color.red);
        if (HitsSomething)
        {
           // Laserbeam.enabled = true;
            Vector3[] BeamLine = new Vector3[] { transform.position, HitHolder.point };
            Laserbeam.SetPositions(BeamLine);
             Debug.Log(transform.position);
            hitObj = HitHolder.transform.gameObject;


        }
        else
        {
            Vector3[] BeamLine = new Vector3[] { transform.position, transform.position+(transform.forward*500)};
            Laserbeam.SetPositions(BeamLine);
        }
    }

   public void SeatAssign()
    {
        Laserbeam.enabled = false;

        if (hitObj && hitObj.tag == "Seat"  )
        {
            //Set seat as taken
          //  hitObj.invoke()

  
                SeatShop.Set("SeatObj", hitObj);
            
        }
      

    }
   
}

