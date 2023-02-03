using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForth : MonoBehaviour
{
    // Start is called before the first frame update
    public int Pathdistance;
    public int stepsize;
    private int start;
    private int endspot;
    

    void Start()
    {
        start = (int)transform.position.z;
          endspot = (int)transform.position.z + Pathdistance;
}

    // Update is called once per frame
    void Update()
    {

        if (transform.position.z>endspot||transform.position.z<start)
        {
            stepsize = stepsize * -1;
           
        }

       
            transform.Translate(0,0,stepsize*Time.deltaTime);
        
    }
}

