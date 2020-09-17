using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class WheelOfFortune : MonoBehaviourPun
{
    private WheelMotor wheelMotor;
    private FortuneWheelPointer fortuneWheelPointer;
    private FortuneWheelMenu fortuneWheelMenu;
    //private AudioSource audioSource;
    private FortuneWheelAudioManager fortuneWheelAudio;


    [SerializeField] private bool stopWheel;
    [SerializeField] private bool startWheel;
    [SerializeField] private float spinTime;

    [SerializeField] private bool spinNow = false;

    [SerializeField] private bool slowRotate = false;
    [SerializeField] private int CostToWin = 300;
    [SerializeField] private bool OfflineMode = false;


    private bool rotateByHand = false;

    private void Start()
    {
        PhotonNetwork.OfflineMode = OfflineMode;
        wheelMotor = GetComponentInChildren<WheelMotor>();
        fortuneWheelPointer = GetComponentInChildren<FortuneWheelPointer>();
        fortuneWheelMenu = GetComponentInChildren<FortuneWheelMenu>();
        fortuneWheelAudio = GetComponent<FortuneWheelAudioManager>();
        wheelMotor.MotorStateChanged += OnMotorStateChanged;
    }

    public void RotateByHand()
    {
        rotateByHand = true;
        wheelMotor.RotateMotorByHand();
        //wheelMotor.transform.Rotate(Vector3.up, 1);
        /*Vector3 currentRotation = wheelMotor.transform.eulerAngles;
        currentRotation.y = Mathf.Lerp(currentRotation.y, currentRotation.y + 1, Time.deltaTime * 10f);
        wheelMotor.transform.eulerAngles = currentRotation;*/

    }

    public void ReleaseHandAndStartMotor()
    {
        if (!spinNow)
        {
            ReleaseHandRotation();
            startWheel = true;
        }
        //startWheel = false;
    }

    public void ReleaseHandRotation()
    {
        //if (!spinNow)
        //{
        rotateByHand = false;
        wheelMotor.StopRotateByHand();
        //}
    }

    private void Update()
    {
        /*if(rotateByHand)
        {
            Transform to = wheelMotor.transform;
            to.localRotation = Quaternion.Euler(new Vector3(to.localEulerAngles.x, to.localEulerAngles.y + 10f, to.localEulerAngles.z));
            Quaternion.Slerp(wheelMotor.transform.localRotation, to.localRotation, Time.deltaTime * 2f);
            rotateByHand = false;
        }*/
        if(startWheel)
        {
            SpinWheel();
            startWheel = false;
        }
        if(spinNow && slowRotate)
        {
            print(string.Format("Slow rotate => Sector => {0}", fortuneWheelPointer.LastSector));
            if (fortuneWheelPointer.LastSector.Cost == CostToWin)
            {
                print("FORCE STOP");
                ForceStopWheel();
            }
        }
    }

    private void ForceStopWheel()
    {
        slowRotate = false;
        wheelMotor.ForceStopMotor();
    }

    private void SpinWheel()
    {
        if(!spinNow)
        {
            //spinNow = true;
            //slowRotate = false;
            photonView.RequestOwnership();
            photonView.RPC("SpinNow_RPC", RpcTarget.All, true);
            photonView.RPC("SlowRotateNow_RPC", RpcTarget.All, true);
            StartCoroutine(SpinWheelWithTime());
        }
    }

    [PunRPC]
    private void SpinNow_RPC(bool spin)
    {
        spinNow = spin;
    }

    [PunRPC]
    private void SlowRotateNow_RPC(bool rotate)
    {
        slowRotate = rotate;
    }

    [PunRPC]
    private void TextInfo_RPC(string text)
    {
        fortuneWheelMenu.SetInfo(text);
    }

    [PunRPC]
    private void PlayFastAudio_RPC(bool play, bool loop)
    {
        fortuneWheelAudio.PlayFastAudio(play, loop);
    }

    [PunRPC]
    private void PlaySlowAudio_RPC(bool play, bool loop)
    {
        fortuneWheelAudio.PlaySlowAudio(play, loop);
    }


    private IEnumerator SpinWheelWithTime()
    {
        wheelMotor.StartMotor();
        yield return new WaitForSeconds(spinTime);
        wheelMotor.StopMotor();
    }

    

    /*[PunRPC]
    private void PlaySpinAudio_RPC(bool play, bool loop)
    {
        print("rpc play wheel audio");
        if (play)
        {
            audioSource.loop = loop;
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }*/

    private void OnMotorStateChanged(WheelMotor.FortuneWheelMotorState stateMotor)
    {
        switch(stateMotor)
        {
            case WheelMotor.FortuneWheelMotorState.INVOKE_STOP:
                print("INVOKED STOP MOTOR");
                break;
            case WheelMotor.FortuneWheelMotorState.STARTED:
                print("INVOKED START MOTOR");
                //fortuneWheelMenu.SetInfo("Spin now");
                photonView.RPC("TextInfo_RPC", RpcTarget.All, "Spin now");
                //PlaySpinAudioClip(true, true);

                photonView.RPC("PlayFastAudio_RPC", RpcTarget.All, true, true);
                //fortuneWheelAudio.PlayFastAudio(true, true);


                break;
            case WheelMotor.FortuneWheelMotorState.TOTAL_STOP:
                //spinNow = false;
                photonView.RPC("SpinNow_RPC", RpcTarget.All, false);
                //PlaySpinAudioClip(false, false);


                photonView.RPC("PlayFastAudio_RPC", RpcTarget.All, false, false);
                //fortuneWheelAudio.PlayFastAudio(false, false);



                print("TOTAL STOP MOTOR");
                print(fortuneWheelPointer.LastSector);
                if (photonView.IsMine)
                {
                    fortuneWheelMenu.SetInfo(string.Format("Your win is {0}$", fortuneWheelPointer.LastSector.Cost), true);
                }
                else
                {
                    fortuneWheelMenu.SetInfo(string.Format("Player {0} win is {1}$", photonView.Owner.NickName, fortuneWheelPointer.LastSector.Cost), true);
                }
                break;
            case WheelMotor.FortuneWheelMotorState.SLOW_ROTATE:
                //slowRotate = true;
                

                photonView.RPC("PlaySlowAudio_RPC", RpcTarget.All, true, false);
                //fortuneWheelAudio.PlaySlowAudio(true, false);


                photonView.RPC("SlowRotateNow_RPC", RpcTarget.All, true);
                print("SLOW ROTATE MOTOR");
                break;
        }
    }
}
