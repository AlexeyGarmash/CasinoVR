﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SlotMachine
{
    public class SlotMachineManager : MonoBehaviour, IListener<SLOT_MACHINE_EVENT>
    {
        public EventManager<SLOT_MACHINE_EVENT> em = new EventManager<SLOT_MACHINE_EVENT>();

        private const int DEFAULT_WIN = 10;
        public GameObject[] reels;
        public SlotMachine slotMachine;      
        public float force;
        public float targetVelocity;
        public float minRellSpeed;
        public float deceleration;
       
        private List<SymbolItem> predictedFruits;

        private bool startRotate = false;
        private int currentReelNumber;
        private string currentTag;

        public bool freeSpin;

       
        void Start()
        {
           
            em.AddListener(SLOT_MACHINE_EVENT.REEL_SYMBOL_TRIGGERED, this);
            em.AddListener(SLOT_MACHINE_EVENT.COIN_INSERTED, this);
            em.AddListener(SLOT_MACHINE_EVENT.BUTTON_PRESSED, this);
            em.AddListener(SLOT_MACHINE_EVENT.BUTTON_PRESSED_JACKPOT, this);
        }

        public void StartSlotGame(bool alwaysJackpot = false)
        {
            if (slotMachine.StartGame())
            {
                predictedFruits = alwaysJackpot ? slotMachine.SameResult() : slotMachine.RandomResult();
                em.PostNotification(SLOT_MACHINE_EVENT.HANDLE_USED, this, null);
                RotateReels();
            }
        }

        private void RotateReels()
        {
            startRotate = true;
            StartCoroutine(ReelsRotate());
        }

        private void StartMotorsOfReels(GameObject reel, float force)
        {
            var hinge = reel.GetComponent<HingeJoint>();
            var rigidbody = reel.GetComponent<Rigidbody>();
            if (hinge != null)
            {
                rigidbody.angularVelocity = Vector3.zero;
                var motor = hinge.motor;
                motor.force = force;
                motor.targetVelocity = force;
                motor.freeSpin = freeSpin;
                hinge.motor = motor;
                hinge.useMotor = true;
            }
        }
        private IEnumerator ReelsRotate()
        {
            em.PostNotification(SLOT_MACHINE_EVENT.REEL_ROTATION_START, this, null);
            //рандомизация конечного результата
            
            //разго барабанов автомата
            foreach (var reel in reels)
            {
                StartMotorsOfReels(reel, force);
            }
            

            //постепенная остановка барабанов
            

            int i = 1;
            //поиск нужного знака на барабане
            foreach (var reel in reels)
            {
                float currSpeed = force;
                while (currSpeed > minRellSpeed)
                {
                    currSpeed -= deceleration;

                    StartMotorsOfReels(reel, currSpeed);

                    yield return new WaitForSeconds(0.05f);
                }
                var hinge = reel.GetComponent<HingeJoint>();
                var rigidbody = reel.GetComponent<Rigidbody>();
                if (hinge != null)
                {
                    hinge.useMotor = false;
                    yield return new WaitUntil(() => i == currentReelNumber && currentTag == predictedFruits[i - 1].Tag);

                    if (i == 1)
                        em.PostNotification(SLOT_MACHINE_EVENT.REELSTOP1, this, predictedFruits[i - 1].Symbol);
                    if (i == 2)
                    {
                        em.PostNotification(SLOT_MACHINE_EVENT.REELSTOP2, this, predictedFruits[i - 1].Symbol);
                        em.PostNotification(SLOT_MACHINE_EVENT.JACKPOT_IS_POSSIBLE, this, null);
                    }
                    if (i == 3)
                        em.PostNotification(SLOT_MACHINE_EVENT.REELSTOP3, this, null);

                    rigidbody.angularVelocity = Vector3.zero;
                    //TODO: Play music of stop reel
                }
                i++;
            }
            em.PostNotification(SLOT_MACHINE_EVENT.REEL_ROTATION_END, this, null);
            //Проверка на соответствие     
            CheckIsWin(predictedFruits);

        }

        private void CheckIsWin(List<SymbolItem> predictedFruits)
        {
            var firstSymbTag = predictedFruits.First().Tag;
            var allAreSame = predictedFruits.All(x => x.Tag == firstSymbTag);
            if (allAreSame)
            {
                em.PostNotification(SLOT_MACHINE_EVENT.JACKPOT_START, this, DEFAULT_WIN * slotMachine.NumberOfCoins);

            }
            else
            {
                em.PostNotification(SLOT_MACHINE_EVENT.NO_JACKPOT, this, null);
            }
            slotMachine.StopGame();
            startRotate = false;
        }

        public void OnEvent(SLOT_MACHINE_EVENT Event_type, Component Sender, params System.Object[] Param)
        {
            switch (Event_type)
            {
                case SLOT_MACHINE_EVENT.COIN_INSERTED:
                    slotMachine.InsertCoin();
                    break;
                case SLOT_MACHINE_EVENT.REEL_SYMBOL_TRIGGERED:
                    currentReelNumber = (int)Param[0];
                    currentTag = (string)Param[1];
                    break;
                case SLOT_MACHINE_EVENT.BUTTON_PRESSED:
                    StartSlotGame();
                    Debug.Log("Button pressed");
                    break;
                case SLOT_MACHINE_EVENT.BUTTON_PRESSED_JACKPOT:
                    StartSlotGame(true);
                    break;
            }
        }
    }

}