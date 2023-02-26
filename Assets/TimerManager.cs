using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerManager : MonoBehaviour
{
    public float timeRemaining;

    public bool timerIsRunning = false;

    public Text lengthOfTime;

    private void Start()

    {

        // Starts the timer automatically

        timerIsRunning = true;

    }

    void Update()

    {
        if (timerIsRunning)

        {

            if (timeRemaining > 0)

            {

                timeRemaining -= Time.deltaTime;

            }
            else
            {

                Debug.Log("Time has run out !");

                timeRemaining = 0;

                timerIsRunning = false;

            }

        }
        lengthOfTime.text = timeRemaining.ToString();

    }
}
