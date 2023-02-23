using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    private void Start()
    {
        timerIsRunning = true;
    }

    // Update is called once per frame
    void Update()
    {
        float min = Mathf.FloorToInt(timeRemaining / 60);
        float sec = Mathf.FloorToInt(timeRemaining % 60);
        if (timerIsRunning)
        {
            if (timeRemaining > 0)

            {

                timeRemaining -= Time.deltaTime;

            }
            else
            {
                Debug.Log("ran out of time");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }

    }
}
