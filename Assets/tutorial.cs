using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tutorial : MonoBehaviour
{
    public float timer = 0;
    public Text text1;
    public bool startMovement = false;
    // Start is called before the first frame update
    void Start()
    {

        //Time.timeScale = 0;
        //text1 = GameObject.FindWithTag("tut dialog 1").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        timer += Time.deltaTime;
        if (startMovement && timer >= 56)
        {
            text1.text = "As you can see the customer goes where you want.";
        }else if (timer >= 56)
        {
            text1.text = "Let's try it out now, hold your right mouse button and hover the pointer at a seat. The light will turn green when on a valid booth. Then let go! ";
            if (Input.GetMouseButtonUp(1))
            {
                startMovement = true;
            }
        }
        else if (timer >= 42)
        {
            text1.text = "Your hands are full, so you'll need to hold the Right mouse button to switch to your laser pointer to show your customer where to sit.";
        }
        else if (timer >= 34)
        {
            text1.text = "Oh! It's opening time, customers are lining up.Hurry! You must seat them as they come in.";
        }
        else if (timer >= 26)
        {
            text1.text = "I'm sure you know how to skate but if not, use the WASD keys to skate around. Go on, try it!";
        }
        else if (timer >= 18)
        {
            text1.text = "Here's your honorary Serve'n'Wich gun and uniform which you must use at all times, especially the skates.";
        }
        else if (timer >= 14)
        {
            text1.text = "Let me train you on the guidelines.";
        }
        else if (timer >= 10)
        {
            text1.text = "Heaven knows the previous guy couldnt... but let's move on.";
        } else if(timer >= 6)
        {
            text1.text = "I'll be your shift lead today, let's hope you can keep up";
        }
        /*if (Input.GetKeyDown(KeyCode.Space) == true)
        {
            Time.timeScale = 1;
        }*/
    }
}
