using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoButtonSound : MonoBehaviour
{
    private AudioSource audioSource;
    private PianoSounds pianoSounds;

    private bool playNow = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        pianoSounds = GetComponentInParent<PianoSounds>();
        audioSource.loop = false;
    }


    public void PlayKeySound(int index)
    {
        if (index != -1)
        {
            audioSource.clip = pianoSounds.GetKeySound(index);
            audioSource.Play();
            playNow = true;
            StartCoroutine(_StopKeySound(1f));
        }
    }

    public void StopKeySound(int index)
    {
        //StartCoroutine(_StopKeySound(0.7f, index));
    }

    private IEnumerator _StopKeySound(float seconds)
    {
        int a = 0;
        yield return new WaitForSeconds(seconds);
        audioSource.Stop();
        audioSource.clip = null;
        playNow = false;


    }


}
