using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toggleTag : MonoBehaviour
{
<<<<<<< Updated upstream
    public void Seated()
    {
        gameObject.tag = "Seats";
        print("customer seated");
    }

    public void Empty()
    {
        gameObject.tag = "";
        print("seat is now empty");
=======
    public void emptySeat()
    {
        gameObject.tag = "Seats";
    }

    public void seatFilled()
    {
        gameObject.tag = "";
>>>>>>> Stashed changes
    }
}
