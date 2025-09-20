using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracing_Audio : MonoBehaviour
{
    //This script deals with audio invovled in tracing logics.

    public AudioClip[] clips;   //stores various audio clips 
    public AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        source.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WrongTraceAudio()
    {
        source.PlayOneShot(clips[2]);
    }
    public void StrokeDoneAudio()
    {
        source.PlayOneShot(clips[1]);
    }
    public void LetterDoneAudio()
    {
        source.PlayOneShot(clips[0]);
    }
    public void TracingSound()
    {
        source.PlayOneShot(clips[3]);
    }
}
