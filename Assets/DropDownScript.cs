using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropDownScript : MonoBehaviour
{
    public GameObject image1;
    public GameObject image2;
    public GameObject image3;

    public AudioSource music1;

    public AudioSource music2;

    public AudioSource music3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void DropDownFunctioning(int value)
    {
        if (value == 0)
        {
            image1.SetActive(false);
            image2.SetActive(false);
            image3.SetActive(false);
            music2.Stop();
            music3.Stop();
            music1.Stop();
        }
        if (value == 1)
        {
            //image1.SetActive(true);
            //image2.SetActive(false);
            //image3.SetActive(false);
            music2.Stop();
            music3.Stop();
            music1.Play();
        }
        if (value == 2)
        {
            //image1.SetActive(false);
            //image2.SetActive(true);
            //image3.SetActive(false);
            music1.Stop();
            music3.Stop();
            music2.Play();
        }
        if (value == 3)
        {
            //image1.SetActive(false);
            //image2.SetActive(false);
            //image3.SetActive(true);
            music1.Stop();
            music2.Stop();
            music3.Play();
        }
    }
}
