using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodTimer : MonoBehaviour
{
    // Start is called before the first frame update
    private bool TimerRunning;
    private float TimerVal = 30;
    private float TimerTick;
    private float TimeMax = 30;
    private SpriteRenderer timerbar;

    void Start()
    {
        timerbar = GetComponent<SpriteRenderer>();
        TimerTick = timerbar.size.x / TimeMax;
    }

    public void SetTimeMax(float timeinput)
    {
        TimeMax = timeinput;
    }
    public void StartTimer()
    {
        TimerRunning = true;
    }
    public void StopTimer()
    {
        TimerRunning = false;
    }
    private void TimerUpdate()
    {
        TimerVal -= Time.deltaTime;
       
        timerbar.size -=  new Vector2(TimerTick,0);
        if (timerbar.size.x <= 0)
        {
            //after sending message to timer frame.make the timer different color and reset amount,
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (TimerRunning == true)
        {
            TimerUpdate();
        }
    }
}


