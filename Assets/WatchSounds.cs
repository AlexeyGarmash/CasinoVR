using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchSounds : MonoBehaviour
{

    AudioSource watches;

    [SerializeField]
    AudioClip signal;

    [SerializeField]
    AudioClip tick;


    public void Start()
    {
        watches = GetComponent<AudioSource>();
    }


    public void PlayTimer(int timeToSignal)
    {
        StopAllCoroutines();
        StartCoroutine(Countdown(timeToSignal));
    }
    public void StopTimer()
    {
        if(watches.isPlaying)
            watches.Stop();
    }

    IEnumerator Countdown(int timeToSignal)
    {
        watches.clip = tick;

        watches.Play();

        yield return new WaitForSeconds(timeToSignal);

        watches.Stop();

        watches.clip = signal;

        watches.Play();

    }
   
}
