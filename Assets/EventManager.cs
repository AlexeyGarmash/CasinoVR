using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SlotMachine
{
    public enum EVENT_TYPE { COIN_INSERTED, JACKPOT_START, JACKPOT_IS_POSSIBLE, JACKPOT_END, REEL_SYMBOL_TRIGGERED, HANDLE_USED, REEL_ROTATION_START, REEL_ROTATION_END, REELSTOP1, REELSTOP2, REELSTOP3, NO_JACKPOT, BUTTON_PRESSED, BUTTON_PRESSED_JACKPOT}


    public interface IListener<T>    {

        void OnEvent(T Event_type, Component Sender, params System.Object[] Param);

    }
    public class EventManager : MonoBehaviour
    {

        #region Properties

        private Dictionary<EVENT_TYPE, List<IListener<EVENT_TYPE>>> Listeners = new Dictionary<EVENT_TYPE, List<IListener<EVENT_TYPE>>>();
        #endregion

        #region Methods
    

        public void AddListener(EVENT_TYPE Event_Type, IListener<EVENT_TYPE> Listener)
        {
            List<IListener<EVENT_TYPE>> ListenList = null;

            if (Listeners.TryGetValue(Event_Type, out ListenList))
            {
                ListenList.Add(Listener);
                return;
            }

            ListenList = new List<IListener<EVENT_TYPE>>();
            ListenList.Add(Listener);
            Listeners.Add(Event_Type, ListenList);
        }

        public void PostNotification(EVENT_TYPE Event_type, Component Sender, params System.Object[] Param)
        {
            List<IListener<EVENT_TYPE>> ListenList = null;

            if (!Listeners.TryGetValue(Event_type, out ListenList))
                return;

            for (int i = 0; i < ListenList.Count; i++)
                if (!ListenList[i].Equals(null))
                    ListenList[i].OnEvent(Event_type, Sender, Param);
        }

        public void RemoveEvent(EVENT_TYPE Event_type)
        {
            Listeners.Remove(Event_type);
        }

        public void RemoveRedundancies()
        {

            Dictionary<EVENT_TYPE, List<IListener<EVENT_TYPE>>> TmpListeners = new Dictionary<EVENT_TYPE, List<IListener<EVENT_TYPE>>>();


            foreach (KeyValuePair<EVENT_TYPE, List<IListener<EVENT_TYPE>>> Item in Listeners)
            {
                for (int i = Item.Value.Count - 1; i >= 0; i--)
                {
                    if (Item.Value[i].Equals(null))
                        Item.Value.RemoveAt(i);
                }

                if (Item.Value.Count > 0)
                    TmpListeners.Add(Item.Key, Item.Value);
            }

            Listeners = TmpListeners;
        }

        void OnLevelWasLoaded()
        {
            RemoveRedundancies();
        }
        #endregion


    }


}