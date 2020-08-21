using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class VoiceManager : MonoBehaviourPun
{
    private PlayerPlace pp;
    private KeywordRecognizer keywordRecognizer;
    private Dictionary<string, Action<PlayerStats>> actions = new Dictionary<string, Action<PlayerStats>>();

    private void Start()
    {

        pp = GetComponent<PlayerPlace>();
       
    }

    public void StopRecognize()
    {
        if (photonView.IsMine)
        {
            keywordRecognizer.Stop();
            actions.Clear();
        }
    }
    public void StartRecognize()
    {
        if (photonView.IsMine)
        {
            if(keywordRecognizer != null)
                keywordRecognizer.Dispose();

            keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());

            keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
            keywordRecognizer.Start();
        }
    }

    public void AddVoiceAction(string words, Action<PlayerStats> action)
    {
        if (photonView.IsMine)
        {
            actions.Add(words, action);
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke(pp.ps);
    }

    
}
