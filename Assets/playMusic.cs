using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playMusic : MonoBehaviour
{
    // Start is called before the first frame update

    public AudioSource audio1;
    public AudioSource audio2;
    public AudioSource audio3;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void play()
    {
        if (audio1.isPlaying)
        {
            audio1.Stop();
        }
        if (audio2.isPlaying)
        {
            audio2.Stop();
        }
        if (audio3.isPlaying)
        {
            audio3.Stop();
        }
    }

}
