using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PianoSounds : MonoBehaviour
{
    public bool isMainKeys = true;
    public AudioClip[] clips;
    public List<string> mainKeys = new List<string>();
    public List<string> secondaryKeys = new List<string>();

    private void Start()
    {
        LoadFromResources();
        mainKeys.Add("__a0");
        mainKeys.Add("__b0");
        secondaryKeys.Add("__a-0");
        GenerateKeys();
    }

    private string[] lettersMain = { "a", "b", "c", "d", "e", "f", "g" };
    private string[] lettersSecond = { "a", "c", "d", "f", "g" };
    private void GenerateKeys()
    {
        if (isMainKeys)
        {
            int k = 0;
            int ind = 1;
            for (int i = 0; i < 50; i++)
            {
                if(k >= 7)
                {
                    k = 0;
                    ind++;
                }
                mainKeys.Add("__" + lettersMain[k] + (ind).ToString());
                k++;
            }
        }
        else
        {
            int k = 0;
            int ind = 1;
            for (int i = 0; i < 35; i++)
            {
                if (k >= 5)
                {
                    k = 0;
                    ind++;
                }
                secondaryKeys.Add("__" + lettersSecond[k] + "-" + (ind).ToString());
                k++;
            }
        }
    }

    private void LoadFromResources()
    {
        clips = Resources.LoadAll<AudioClip>("Audio/piano_sounds");
    }

    public AudioClip GetKeySound(int index)
    {
        string strIndex = isMainKeys ? mainKeys[index] : secondaryKeys[index];
        AudioClip findClip = clips.FirstOrDefault(cl => cl.name.Contains(strIndex));
        return findClip;
    }

}
