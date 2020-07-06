using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class SoundManager : MonoBehaviour, IListener<EVENT_TYPE>
    {

        public AudioSource Jackpot;
        public AudioSource ReelRotation;
        public AudioSource ReelStop1;
        public AudioSource ReelStop2;
        public AudioSource ReelStop3;
        public AudioSource Handle;
        public AudioSource Coin;

        EventManager em;

        void Start()
        {

            em = gameObject.GetComponent<EventManager>();

            em.AddListener(EVENT_TYPE.COIN_INSERTED, this);
            em.AddListener(EVENT_TYPE.JACKPOT_START, this);
            em.AddListener(EVENT_TYPE.JACKPOT_END, this);
            em.AddListener(EVENT_TYPE.REELSTOP1, this);
            em.AddListener(EVENT_TYPE.REELSTOP2, this);
            em.AddListener(EVENT_TYPE.REELSTOP3, this);
            em.AddListener(EVENT_TYPE.HANDLE_USED, this);
            em.AddListener(EVENT_TYPE.REEL_ROTATION_START, this);
            em.AddListener(EVENT_TYPE.REEL_ROTATION_END, this);

        }

        public void OnEvent(EVENT_TYPE Event_type, Component Sender, params object[] Param)
        {
            switch (Event_type)
            {
                case EVENT_TYPE.COIN_INSERTED:
                    Coin.Play(0);
                    break;
                case EVENT_TYPE.JACKPOT_START:
                    Jackpot.Play(0);
                    break;
                case EVENT_TYPE.JACKPOT_END:
                    Jackpot.Stop();
                    break;
                case EVENT_TYPE.REELSTOP1:
                    ReelStop1.Play(0);
                    break;
                case EVENT_TYPE.REELSTOP2:
                    ReelStop2.Play(0);
                    break;
                case EVENT_TYPE.REELSTOP3:
                    ReelStop3.Play(0);
                    break;
                case EVENT_TYPE.HANDLE_USED:
                    Handle.Play(0);
                    break;
                case EVENT_TYPE.REEL_ROTATION_START:
                    ReelRotation.Play(0);
                    break;
                case EVENT_TYPE.REEL_ROTATION_END:
                    ReelRotation.Stop();
                    break;

            }
        }

        

    }

}