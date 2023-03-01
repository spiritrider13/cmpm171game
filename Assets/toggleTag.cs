using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleTag : MonoBehaviour
{
    [SerializeField]
    public Transform moveSpot;

    public bool Seated()
    {
        gameObject.tag = "Seats";
        print("seat is empty");
        return true;
    }

    public bool Empty()
    {
        gameObject.tag = "Untagged";
        print("seat is filled");
        return false;
    }

    public Vector3 returnMoveSpotPosition()
    {
        return moveSpot.transform.position;
    }
}
