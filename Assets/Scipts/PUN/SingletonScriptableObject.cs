using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class SingletonScriptableObject<T>: ScriptableObject where T: ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(!_instance)
            {
                _instance = Resources.FindObjectsOfTypeAll<T>().FirstOrDefault();
            }
            return _instance;
        }
    }
}
