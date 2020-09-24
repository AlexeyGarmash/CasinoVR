using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FortuneWheelAudioManager : MonoBehaviourPun
{
    [SerializeField] private AudioClip FastSpin;
    [SerializeField] private AudioClip SlowSpin;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        //ClearClips();
    }



    public void PlayFastAudio(bool play, bool loop)
    {
        ClearClips();
        audioSource.clip = FastSpin;
        ResolvePlayAudio(play, loop);
    }

    public void PlaySlowAudio(bool play, bool loop)
    {
        ClearClips();
        audioSource.clip = SlowSpin;
        ResolvePlayAudio(play, loop);
    }

    private void ResolvePlayAudio(bool play, bool loop)
    {
        audioSource.loop = loop;
        if (play)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ClearClips()
    {
        audioSource.Stop();
        audioSource.clip = null;
        audioSource.loop = false;
    }
}
