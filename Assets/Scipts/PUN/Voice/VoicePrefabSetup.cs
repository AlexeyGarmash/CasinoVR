using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoicePrefabSetup : MonoBehaviour
{
    [SerializeField] private PhotonView photonView;
    [SerializeField] private PhotonVoiceView photonVoiceView;
    [SerializeField] private Speaker speaker;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        photonVoiceView = GetComponent<PhotonVoiceView>();
        speaker = GetComponent<Speaker>();
    }

    public void SetupPhotonVoice(Player photonPlayer)
    {
        photonView.ViewID = photonPlayer.ActorNumber * 1000;
    }
}
