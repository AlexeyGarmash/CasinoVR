using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

    /*public static string GetPrefabPath(this GameObject go)
    {
        return PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
    }

    public static string GetResourceName(this GameObject go)
    {
        string fullPath = PrefabUtility.GetPrefabAssetPathOfNearestInstanceRoot(go);
        if (fullPath.ToLower().Contains("resources"))
        {
            int indexResStart = fullPath.ToLower().IndexOf("resources");
            indexResStart += 10;
            string resourceNameWithExt = fullPath.Substring(indexResStart);
            int indexStop = resourceNameWithExt.LastIndexOf(".");
            resourceNameWithExt = resourceNameWithExt.Substring(0, indexStop);
            return resourceNameWithExt;
        }
        return null;
    }*/
   
}
