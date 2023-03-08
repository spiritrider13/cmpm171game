using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class doorBell : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public class PlayOnCollision : MonoBehaviour
    {
        public AudioSource audioSource;
        private void OnTriggerEnter(Collider other)
        {
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
    }
}
