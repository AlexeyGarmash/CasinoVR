using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum SLOT_MACHINE_EVENT { COIN_INSERTED, JACKPOT_START, JACKPOT_IS_POSSIBLE, JACKPOT_END, REEL_SYMBOL_TRIGGERED, HANDLE_USED, REEL_ROTATION_START, REEL_ROTATION_END, REELSTOP1, REELSTOP2, REELSTOP3, NO_JACKPOT, BUTTON_PRESSED, BUTTON_PRESSED_JACKPOT}


public interface IListener<T> where T : System.Enum    {

    void OnEvent(T Event_type, Component Sender, params System.Object[] Param);

}
public class EventManager<T> where T : System.Enum
{

    #region Properties

    private Dictionary<T, List<IListener<T>>> Listeners = new Dictionary<T, List<IListener<T>>>();
    #endregion

    #region Methods
    

    public void AddListener(T Event_Type, IListener<T> Listener)
    {
        List<IListener<T>> ListenList = null;

        if (Listeners.TryGetValue(Event_Type, out ListenList))
        {
            ListenList.Add(Listener);
            return;
        }

        ListenList = new List<IListener<T>>();
        ListenList.Add(Listener);
        Listeners.Add(Event_Type, ListenList);
    }


    public void PostNotification(T Event_type, Component Sender, params System.Object[] Param)
    {
        List<IListener<T>> ListenList = null;

        if (!Listeners.TryGetValue(Event_type, out ListenList))
            return;

        for (int i = 0; i < ListenList.Count; i++)
            if (!ListenList[i].Equals(null))
                ListenList[i].OnEvent(Event_type, Sender, Param);
    }

    public void RemoveEvent(T Event_type)
    {
        Listeners.Remove(Event_type);
    }

    public void RemoveRedundancies()
    {

        Dictionary<T, List<IListener<T>>> TmpListeners = new Dictionary<T, List<IListener<T>>>();


        foreach (KeyValuePair<T, List<IListener<T>>> Item in Listeners)
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

