using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SlotMachine
{

    public class CoinsSpawner : MonoBehaviour, IListener<SLOT_MACHINE_EVENT>
    {
        public GameObject[] SpawnPoints;
        public GameObject SpawnCoin;
        public int MaxCoins;
        public float CoinForce = 10;
        private int currentCoinsCount = 0;
        EventManager<SLOT_MACHINE_EVENT> em;
        void Start()
        {
            em = gameObject.transform.parent.GetComponent<SlotMachineManager>().em;
            em.AddListener(SLOT_MACHINE_EVENT.JACKPOT_START, this);
        }
        

        void SpawnCoins()
        {      
            StartCoroutine(_SpawnCoin());          
        }



        IEnumerator _SpawnCoin()
        {
            currentCoinsCount = 0;

            while (currentCoinsCount <= MaxCoins)
            {
                int index = Random.Range(0, SpawnPoints.Length);
                var spawnPoint = SpawnPoints[index];
                GameObject coin = Instantiate(SpawnCoin, spawnPoint.transform.position, Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
                coin.GetComponent<Rigidbody>().AddForce(-spawnPoint.transform.up * CoinForce);
                currentCoinsCount++;
                //Destroy(coin, 5f);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));
            }
            
            yield return new WaitForSeconds(2f);
            em.PostNotification(SLOT_MACHINE_EVENT.JACKPOT_END, this, null);

        }


        public void OnEvent(SLOT_MACHINE_EVENT Event_type, Component Sender, params System.Object[] Param)
        {
            switch (Event_type)
            {
                case SLOT_MACHINE_EVENT.JACKPOT_START:
                    currentCoinsCount = 0;
                    MaxCoins = (int)Param[0];
                    SpawnCoins();
                    break;

            }
        }

    }

}