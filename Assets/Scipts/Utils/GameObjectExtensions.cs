using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtensions
{

    public static void SetScale(this GameObject go, Vector3 scale)
    {
        go.transform.localScale = scale;
    }

    public static void SetScale(this GameObject go, float x, float y, float z)
    {
        go.transform.localScale = new Vector3(x, y, z);
    }
   
}
