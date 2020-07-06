using SlotMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace SlotMachine
{
    public enum SLOT_MACHINE_LAMP_MODS { IDLE, FAST,  ALL_LAMP_RANDOM_SINGLE_COLOR_ONCE, ALL_LAMP_RANDOM_SINGLE_COLOR, STOP, ALL_LAMP_RANDOM_ONCE, ALL_LAMP_RANDOM, NONE, MIXED }
    public class LightSystem : MonoBehaviour, IListener<EVENT_TYPE>
    {

        SLOT_MACHINE_LAMP_MODS prev = SLOT_MACHINE_LAMP_MODS.NONE;
        SLOT_MACHINE_LAMP_MODS current = SLOT_MACHINE_LAMP_MODS.NONE;

        const float LOW_DELAY = 0.1f;
        const float NORMAL_DELAY = 0.5f;
        const float HIGH_DELAY = 0.8f;

        public GameObject[] Lamps;

        public Color[] HaloColors;
        public Color[] MaterialColors;

        EventManager em;
        void Start()
        {
            em = gameObject.transform.parent.GetComponent<EventManager>();

            AddListeners();
            GetLamps();

            ModChanger(SLOT_MACHINE_LAMP_MODS.IDLE);

        }
        private void GetLamps()
        {
            Lamps = new GameObject[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
                Lamps[i] = transform.GetChild(i).gameObject;
        }
        private void AddListeners()
        {
            em.AddListener(EVENT_TYPE.REEL_ROTATION_START, this);
            em.AddListener(EVENT_TYPE.REEL_ROTATION_END, this);
            em.AddListener(EVENT_TYPE.REELSTOP1, this);
            em.AddListener(EVENT_TYPE.REELSTOP2, this);
            em.AddListener(EVENT_TYPE.REELSTOP3, this);
            em.AddListener(EVENT_TYPE.NO_JACKPOT, this);
            em.AddListener(EVENT_TYPE.JACKPOT_IS_POSSIBLE, this);
            em.AddListener(EVENT_TYPE.JACKPOT_START, this);
            em.AddListener(EVENT_TYPE.JACKPOT_END, this);
        }


        private void ModChanger(SLOT_MACHINE_LAMP_MODS mod)
        {
            prev = current;
            current = mod;

            StopAllCoroutines();

            if (mod == SLOT_MACHINE_LAMP_MODS.IDLE)           
                IdleMode();            
            else if (mod == SLOT_MACHINE_LAMP_MODS.FAST)           
                FastMode();            
            else if (mod == SLOT_MACHINE_LAMP_MODS.ALL_LAMP_RANDOM_SINGLE_COLOR_ONCE)           
                OneColorShot(NORMAL_DELAY);
            else if (mod == SLOT_MACHINE_LAMP_MODS.ALL_LAMP_RANDOM_SINGLE_COLOR)
                AllLampsRandomSingleColor(LOW_DELAY);
            else if (mod == SLOT_MACHINE_LAMP_MODS.ALL_LAMP_RANDOM_ONCE)
                AllLampsRandomOnce(HIGH_DELAY);
            else if (mod == SLOT_MACHINE_LAMP_MODS.ALL_LAMP_RANDOM)
                AllLampsRandom(LOW_DELAY);
            else if (mod == SLOT_MACHINE_LAMP_MODS.MIXED)
                MixedMode(LOW_DELAY);
            else StopMode();
            
            
        }

        private void SetAllSingleColor(Color Halo, Color Lamp)
        {
            for (int i = 0; i < Lamps.Length; i++)
            {
                SetSingleColor(Halo, Lamp, Lamps[i]);
            }
        }
        private void SetAllRandomColor()
        {
            int randC;
            int prevC = 0;
            for (var i = 0; i < Lamps.Length; i++)
            {
                randC = UnityEngine.Random.Range(0, HaloColors.Length);
                while (randC == prevC)
                    randC = UnityEngine.Random.Range(0, HaloColors.Length);

                SetSingleColor(HaloColors[randC], MaterialColors[randC], Lamps[i]);
                prevC = randC;
            }
        }
        private void SetSingleColor(Color NextHaloColor, Color NextLampColor, GameObject Lamp)
        {
            var material = Lamp.GetComponent<Renderer>().materials[1];
            SerializedObject halo = new SerializedObject(Lamp.transform.GetChild(0).gameObject.GetComponent("Halo"));

            halo.FindProperty("m_Color").colorValue = NextHaloColor;
            halo.ApplyModifiedProperties();
            material.color = NextLampColor;
        }

        private IEnumerator SequenceChange(float delay)
        {
            int c = 0;
            int count = Lamps.Length;

            while (true)
            {
                for (int i = 0; i < count; i++)
                {

                    if (c >= 2)
                        c = 0;
                    else
                        c++;

                    SetSingleColor(HaloColors[c], MaterialColors[c], Lamps[i]);
                }
                if (c >= 2)
                    c = 0;
                else
                    c++;
                yield return new WaitForSeconds(delay);
            }
        }

        private void IdleMode()
        {
            
            StartCoroutine(SequenceChange(NORMAL_DELAY));
        }
        private void FastMode()
        {
            
            StartCoroutine(SequenceChange(LOW_DELAY));
        }

        private IEnumerator _OneShotColor(float delay)
        {

            int randC = UnityEngine.Random.Range(0, HaloColors.Length);
            SetAllSingleColor(HaloColors[randC], MaterialColors[randC]);

            yield return new WaitForSeconds(delay);
            ModChanger(prev);
        }
        private void OneColorShot(float delay)
        {
            
            StartCoroutine(_OneShotColor(delay));
        }


        private IEnumerator _AllLampsRandomSingleColor(float delay)
        {
            int randC;
            int prevC = 0;
            while (true)
            {
                randC = UnityEngine.Random.Range(0, HaloColors.Length);

                while (randC == prevC)
                    randC = UnityEngine.Random.Range(0, HaloColors.Length);
                SetAllSingleColor(HaloColors[randC], MaterialColors[randC]);

                prevC = randC;

                yield return new WaitForSeconds(delay);
                
            }
        }
        private void AllLampsRandomSingleColor(float delay)
        {

            StartCoroutine(_AllLampsRandomSingleColor(delay));
        }
        private IEnumerator _AllLampsRandomSingleColorOnce(float delay)
        {
            int randC;       
            randC = UnityEngine.Random.Range(0, HaloColors.Length);            
            SetAllSingleColor(HaloColors[randC], MaterialColors[randC]);
            yield return new WaitForSeconds(delay);
            ModChanger(prev);

        }
        private void AllLampsRandomSingleColorOnce(float delay)
        {
            StartCoroutine(_AllLampsRandomSingleColorOnce(delay));
        }

        private void StopMode()
        {
            
            StartCoroutine(_StopMode());
        }
        private IEnumerator _StopMode()
        {
            yield return new WaitForSeconds(2f);
            ModChanger(SLOT_MACHINE_LAMP_MODS.IDLE);
        }

        
        private void AllLampsRandom(float delay)
        {

            StartCoroutine(_AllLampsRandom(delay));
        }
        private IEnumerator _AllLampsRandom(float delay)
        {
            
            while (true)
            {
                SetAllRandomColor();
                yield return new WaitForSeconds(delay);

            }
        }
        private void AllLampsRandomOnce(float delay)
        {

            StartCoroutine(_AllLampsRandomOnce(delay));
        }
        private IEnumerator _AllLampsRandomOnce(float delay)
        {
            SetAllRandomColor();
            yield return new WaitForSeconds(delay);
            ModChanger(prev);
        }

        private void MixedMode(float delay)
        {
            StartCoroutine(_MixedMode(delay));
        }
        private IEnumerator _MixedMode(float delay)
        {
            while (true)
            {
                var value = UnityEngine.Random.Range(0, 2);
                if (value == 0)
                {
                    int randC;
                    randC = UnityEngine.Random.Range(0, HaloColors.Length);
                    SetAllSingleColor(HaloColors[randC], MaterialColors[randC]);
                    
                }
                else SetAllRandomColor();
                yield return new WaitForSeconds(delay);
            }
        }


        public void OnEvent(EVENT_TYPE Event_type, Component Sender, params object[] Param)
        {

            switch (Event_type)
            {
                case EVENT_TYPE.REEL_ROTATION_START:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.FAST);                   
                    break;
                case EVENT_TYPE.REEL_ROTATION_END:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.IDLE);                    
                    break;
                case EVENT_TYPE.REELSTOP1:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.ALL_LAMP_RANDOM_SINGLE_COLOR_ONCE);
                    break;
                                
                case EVENT_TYPE.JACKPOT_IS_POSSIBLE:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.ALL_LAMP_RANDOM_SINGLE_COLOR);
                    break;
                case EVENT_TYPE.NO_JACKPOT:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.STOP);
                    break;
                case EVENT_TYPE.JACKPOT_START:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.MIXED);

                    break;
                case EVENT_TYPE.JACKPOT_END:
                    ModChanger(SLOT_MACHINE_LAMP_MODS.IDLE);
                    break;
            }
        }
    }

}