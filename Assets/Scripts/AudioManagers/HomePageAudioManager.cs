using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomePageAudioManager : MonoBehaviour
{
    //This script handles button clicks at Home Page


    public AudioClip click;
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlayClickSound()
    {
        source.PlayOneShot(click);
    }
}
