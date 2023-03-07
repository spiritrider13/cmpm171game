using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float countdown = 5;
    public Text timeText;
    
    // Start is called before the first frame update
    void Start()
    {
        timeText.text = "New customer in 5 seconds.";
    }

    // Update is called once per frame
    void Update()
    {
        //puts countdown in the log
        Debug.Log(countdown);

        //decrease while greater than 0
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
        }
        else {
            Debug.Log("spawn");
            

        }

        //display
       timeOnScreen(countdown);
    }

    void timeOnScreen(float timeDisplay)
    {
        timeDisplay += 1;
        float sec = Mathf.FloorToInt(timeDisplay % 60);
        timeText.text = string.Format("New customer in {0:00}", sec) + " seconds.";

    }
}
