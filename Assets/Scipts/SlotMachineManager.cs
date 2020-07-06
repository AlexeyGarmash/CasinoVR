using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SlotMachine
{
    public class SlotMachineManager : MonoBehaviour, IListener<EVENT_TYPE>
    {
        private const int DEFAULT_WIN = 10;
        public GameObject[] reels;
        public SlotMachine slotMachine;      
        public float force;
        public float targetVelocity;
        public float minRellSpeed;
        public float deceleration;
        EventManager em;

        private List<SymbolItem> predictedFruits;

        private bool startRotate = false;
        private int currentReelNumber;
        private string currentTag;

        public bool freeSpin;

        void Start()
        {
            em = GetComponent<EventManager>();
            em.AddListener(EVENT_TYPE.REEL_SYMBOL_TRIGGERED, this);
            em.AddListener(EVENT_TYPE.COIN_INSERTED, this);
            em.AddListener(EVENT_TYPE.BUTTON_PRESSED, this);
            em.AddListener(EVENT_TYPE.BUTTON_PRESSED_JACKPOT, this);
        }

        public void StartSlotGame(bool alwaysJackpot = false)
        {
            if (slotMachine.StartGame())
            {
                predictedFruits = alwaysJackpot ? slotMachine.SameResult() : slotMachine.RandomResult();
                em.PostNotification(EVENT_TYPE.HANDLE_USED, this, null);
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
            em.PostNotification(EVENT_TYPE.REEL_ROTATION_START, this, null);
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
                        em.PostNotification(EVENT_TYPE.REELSTOP1, this, predictedFruits[i - 1].Symbol);
                    if (i == 2)
                    {
                        em.PostNotification(EVENT_TYPE.REELSTOP2, this, predictedFruits[i - 1].Symbol);
                        em.PostNotification(EVENT_TYPE.JACKPOT_IS_POSSIBLE, this, null);
                    }
                    if (i == 3)
                        em.PostNotification(EVENT_TYPE.REELSTOP3, this, null);

                    rigidbody.angularVelocity = Vector3.zero;
                    //TODO: Play music of stop reel
                }
                i++;
            }
            em.PostNotification(EVENT_TYPE.REEL_ROTATION_END, this, null);
            //Проверка на соответствие     
            CheckIsWin(predictedFruits);

        }

        private void CheckIsWin(List<SymbolItem> predictedFruits)
        {
            var firstSymbTag = predictedFruits.First().Tag;
            var allAreSame = predictedFruits.All(x => x.Tag == firstSymbTag);
            if (allAreSame)
            {
                em.PostNotification(EVENT_TYPE.JACKPOT_START, this, DEFAULT_WIN * slotMachine.NumberOfCoins);

            }
            else
            {
                em.PostNotification(EVENT_TYPE.NO_JACKPOT, this, null);
            }
            slotMachine.StopGame();
            startRotate = false;
        }

        public void OnEvent(EVENT_TYPE Event_type, Component Sender, params System.Object[] Param)
        {
            switch (Event_type)
            {
                case EVENT_TYPE.COIN_INSERTED:
                    slotMachine.InsertCoin();
                    break;
                case EVENT_TYPE.REEL_SYMBOL_TRIGGERED:
                    currentReelNumber = (int)Param[0];
                    currentTag = (string)Param[1];
                    break;
                case EVENT_TYPE.BUTTON_PRESSED:
                    StartSlotGame();
                    Debug.Log("Button pressed");
                    break;
                case EVENT_TYPE.BUTTON_PRESSED_JACKPOT:
                    StartSlotGame(true);
                    break;
            }
        }
    }

}