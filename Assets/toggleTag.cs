using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleTag : MonoBehaviour
{
    public void Seated()
    {
        gameObject.tag = "Seats";
        print("customer seated");
    }

    public void Empty()
    {
        gameObject.tag = "";
        print("seat is now empty");
    }
}
