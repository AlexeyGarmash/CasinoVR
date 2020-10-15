using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ColorExtensions 
{
    public static string ToStringColor(this Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGBA(color);
    }


    public static Color FromStringColor(string stringColor)
    {
        Color retColor;
        ColorUtility.TryParseHtmlString(stringColor, out retColor);
        return retColor;
    }
}
