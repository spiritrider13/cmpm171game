using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleTag : MonoBehaviour
{
    [SerializeField]
    public Transform moveSpot;

    public bool Empty()
    {
        gameObject.tag = "Seats";
        print("seat is empty");
        return true;
    }

    public bool Seated()
    {
        gameObject.tag = "Untagged";
        print("seat is filled");
        return false;
    }

    public Vector3 returnMoveSpotPosition()
    {
        return moveSpot.transform.position;
    }
   // public void OnCollisionExit(Collision collision)
   // {

    //}

}
