using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{
    public class SoundManager : MonoBehaviour, IListener<SLOT_MACHINE_EVENT>
    {

        public AudioSource Jackpot;
        public AudioSource ReelRotation;
        public AudioSource ReelStop1;
        public AudioSource ReelStop2;
        public AudioSource ReelStop3;
        public AudioSource Handle;
        public AudioSource Coin;

        EventManager<SLOT_MACHINE_EVENT> em;

        void Start()
        {

            em = gameObject.GetComponent<SlotMachineManager>().em;

            em.AddListener(SLOT_MACHINE_EVENT.COIN_INSERTED, this);
            em.AddListener(SLOT_MACHINE_EVENT.JACKPOT_START, this);
            em.AddListener(SLOT_MACHINE_EVENT.JACKPOT_END, this);
            em.AddListener(SLOT_MACHINE_EVENT.REELSTOP1, this);
            em.AddListener(SLOT_MACHINE_EVENT.REELSTOP2, this);
            em.AddListener(SLOT_MACHINE_EVENT.REELSTOP3, this);
            em.AddListener(SLOT_MACHINE_EVENT.HANDLE_USED, this);
            em.AddListener(SLOT_MACHINE_EVENT.REEL_ROTATION_START, this);
            em.AddListener(SLOT_MACHINE_EVENT.REEL_ROTATION_END, this);

        }

        public void OnEvent(SLOT_MACHINE_EVENT Event_type, Component Sender, params object[] Param)
        {
            switch (Event_type)
            {
                case SLOT_MACHINE_EVENT.COIN_INSERTED:
                    Coin.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.JACKPOT_START:
                    Jackpot.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.JACKPOT_END:
                    Jackpot.Stop();
                    break;
                case SLOT_MACHINE_EVENT.REELSTOP1:
                    ReelStop1.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.REELSTOP2:
                    ReelStop2.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.REELSTOP3:
                    ReelStop3.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.HANDLE_USED:
                    Handle.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.REEL_ROTATION_START:
                    ReelRotation.Play(0);
                    break;
                case SLOT_MACHINE_EVENT.REEL_ROTATION_END:
                    ReelRotation.Stop();
                    break;

            }
        }

        

    }

}