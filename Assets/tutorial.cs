using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    public float timer;
    public Text dialogue;
    public Text shadow;
    public bool startInteraction = false;
    public bool ePushed = false;
    public TimerManager timerM;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (startInteraction && ePushed && timer >= 105)
        {
            dialogue.text = "";
            shadow.text = "";
            timerM.timerIsRunning = true;
        }
        else if (startInteraction && ePushed && timer >= 100)
        {
            dialogue.text = "Alright toodleoo!";
            shadow.text = "Alright toodleoo!";
        }
        else if (startInteraction && ePushed && timer >= 90)
        {
            dialogue.text = "Alright, that's it for me now. Good luck rookie! I'll be on my vacation now, don't bother reaching out to me, I won't answer.";
            shadow.text = "Alright, that's it for me now. Good luck rookie! I'll be on my vacation now, don't bother reaching out to me, I won't answer.";
        }
        else if (startInteraction && ePushed && timer >= 86)
        {
            dialogue.text = "Once the timer runs out, you can go home. For the day.";
            shadow.text = "Once the timer runs out, you can go home. For the day.";
        }
        else if (startInteraction && ePushed && timer >= 77)
        {
            dialogue.text = "Down here's the timer. Surely you didn't think that your shift lasted forever did you? Hahaha... no. That would cost us thousands in insurance fees.";
            shadow.text = "Down here's the timer. Surely you didn't think that your shift lasted forever did you? Hahaha... no. That would cost us thousands in insurance fees.";
        }
        else if (startInteraction && ePushed && timer >= 73)
        {
            dialogue.text = "There's a few more icons you should know about";
            shadow.text = "There's a few more icons you should know about";
        }
        else if (startInteraction && ePushed && timer >= 69)
        {
            dialogue.text = "You can check your tips up here in the top left corner.";
            shadow.text = "You can check your tips up here in the top left corner.";
        }
        else if (startInteraction && ePushed && timer >= 63)
        {
            dialogue.text = "Look, they even left a tip. Go towards it and press E to pick it up";
            shadow.text = "Look, they even left a tip. Go towards it and press E to pick it up";
        }
        else if (startInteraction && ePushed && timer >= 60)
        {
            dialogue.text = "See? That wasn't so hard!";
            shadow.text = "See? That wasn't so hard!";
        } else if (startInteraction && ePushed)
        {
            dialogue.text = "Now face the customer and shoot the dish at them by clicking the Left Mouse Button.";
            shadow.text = "Now face the customer and shoot the dish at them by clicking the Left Mouse Button.";
        }
        else if(startInteraction && timer >= 51)
        {

            dialogue.text = "Pick it up using the E button.";
            shadow.text = "Pick it up using the E button.";
            if (Input.GetKeyDown(KeyCode.E))
            {
                ePushed = true;
            }
        }else if (startInteraction && timer >= 45)
        {
            dialogue.text = "Now, we need to feed our customer. Turn around and you will find some food.";
            shadow.text = "Now, we need to feed our customer. Turn around and you will find some food.";
        }
        else if (startInteraction)
        {
            dialogue.text = "As you can see the customer goes where you want.";
            shadow.text = "As you can see the customer goes where you want.";
            //timer = 50;
        }
        else if(timer >= 37)
        {
            if (Input.GetMouseButtonUp(1))
            {
                startInteraction = true;
            }
            dialogue.text = "Let's try it out now, hold your right mouse button and hover the pointer at a seat. The light will turn green when on a valid booth. Then let go!";
            shadow.text = "Let's try it out now, hold your right mouse button and hover the pointer at a seat. The light will turn green when on a valid booth. Then let go!";
        }
        else if (timer >= 31)
        {
            dialogue.text = "You're going to need to hold the Right mouse button to switch to your laser pointer to show your customer where to sit.";
            shadow.text = "You're going to need to hold the Right mouse button to switch to your laser pointer to show your customer where to sit.";
        }
        else if (timer >= 27)
        {
            dialogue.text = "Oh! It's opening time, customers are lining up. Hurry! You must seat them as they come in.";
            shadow.text = "Oh! It's opening time, customers are lining up. Hurry! You must seat them as they come in.";
        }
        else if (timer >= 22)
        {
            dialogue.text = "I'm sure you know how to skate already, but just in case you don't, Use the WASD keys to skate around.";
            shadow.text = "I'm sure you know how to skate already, but just in case you don't, Use the WASD keys to skate around.";
        }
        else if (timer >= 16)
        {
            dialogue.text = "Here's your honorary Serve'n'Wich gun and uniform which you must use at all times, especially the skates.";
            shadow.text = "Here's your honorary Serve'n'Wich gun and uniform which you must use at all times, especially the skates.";
        }
        else if (timer >= 12)
        {
            dialogue.text = "Let me train you on the guidelines.";
            shadow.text = "Let me train you on the guidelines.";
        }
        else if (timer >= 8)
        {
            dialogue.text = "Heaven knows the previous guy couldnt... but let's move on.";
            shadow.text = "Heaven knows the previous guy couldnt... but let's move on.";
        } else if (timer >= 4)
        {
            dialogue.text = "I'll be your shift lead today, let's hope you can keep up";
            shadow.text = "I'll be your shift lead today, let's hope you can keep up";
        }
    }
}
