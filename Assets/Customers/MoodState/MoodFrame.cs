using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodFrame : MonoBehaviour
{
    enum Mood
    {
        Happy,
        Bored,
        Annoyed
    }
    private SpriteRenderer mood_frame;
    public Sprite[] framesprites;
    public int tiptype = 0;
    // Start is called before the first frame update
    private Mood currmood = Mood.Happy;

    void Start()
    {
        mood_frame = GetComponent<SpriteRenderer>();
       // framesprites = { "Satis"}
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   public void ChangeStateMood()
    {
        switch (currmood)
        {
            case Mood.Happy:
                currmood = Mood.Bored;
                mood_frame.sprite = framesprites[1];
                tiptype = 1;
               // BroadcastMessage("TimerColor", Color.yellow);
                break;
            case Mood.Bored:
                currmood = Mood.Annoyed;
                mood_frame.sprite = framesprites[2];
                tiptype = 2;
                //BroadcastMessage("TimerColor", Color.red);
                break;
            case Mood.Annoyed:
                transform.parent.SendMessageUpwards("ExitDiner");

                break;
            default:
                break;
        }
    }
}
