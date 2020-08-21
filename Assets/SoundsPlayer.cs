using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsPlayer : MonoBehaviour
{
    [SerializeField]
    List<AudioClip> clips;

    private AudioClip defaultClip;

    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        defaultClip = source.clip;
    }

    public void PlayRandomClip()
    {
        if(source.isPlaying)
            source.Stop();

        if (clips.Count > 0)
        {

            source.clip = clips[Random.Range(0, clips.Count - 1)];
        }       

        source.Play();
    }
    public void PlayDefaultClip()
    {
        source.Stop();

        if (defaultClip != source.clip)
        {

            source.clip = defaultClip;
        }

        source.Play();
    }
}
