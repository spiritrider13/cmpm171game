using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodTimer : MonoBehaviour
{
    // Start is called before the first frame update
    //Attach this to the bar that changes

    private bool TimerRunning;
    private int timeschanged =0;
    private float TimerVal = 30;
    private float TimerTick = 0;
    private float TimeMax = 30;
    private Vector2 timebaroriginal;
    private Transform timerbar;
    private SpriteRenderer timercolor;

    void Start()
    {
        timerbar = GetComponent<Transform>();
        TimerTick = timerbar.localScale.x / TimeMax;
        timercolor = GetComponent<SpriteRenderer>();
        timebaroriginal =  new Vector3(timerbar.localScale.x,timerbar.localScale.y,timerbar.localScale.z);
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
       // TimerVal -= Time.deltaTime;
       
        timerbar.localScale -=  new Vector3(TimerTick*Time.deltaTime,0,0);
      //  timerbar.localPosition -= new Vector3((TimerTick * Time.deltaTime)/2, 0, 0);
        if (timerbar.localScale.x <= 0 )
        {
            if (timeschanged == 0)
            {
                timercolor.color = Color.yellow;
                transform.parent.BroadcastMessage("ChangeStateMood");


                TimerVal = TimeMax;
                timerbar.localScale = timebaroriginal;
          //      timerbar.localPosition += new Vector3(1, 0, 0);
            }
            if (timeschanged == 1)
            {
                transform.parent.BroadcastMessage("ChangeStateMood");

                timercolor.color = Color.red;
                TimerVal = TimeMax;
                timerbar.localScale = timebaroriginal;
              //  timerbar.localPosition += new Vector3(1, 0, 0);
            }
            if(timeschanged == 2)
            {
                transform.parent.BroadcastMessage("ChangeStateMood");
                TimerRunning = false;
            }
                timeschanged++;
            
           

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


