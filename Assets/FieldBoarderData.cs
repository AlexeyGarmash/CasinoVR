using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldBoarderData : MonoBehaviour
{
    private CurvedLinePoint[] points;
    private Vector2[] polygon;
    void Start()
    {
        points = GetComponentsInChildren<CurvedLinePoint>();
        polygon = new Vector2[points.Length];
        for (var i = 0; i < points.Length; i++)
            polygon[i] = new Vector2(points[i].transform.position.x, points[i].transform.position.z);
    }

    //Для ограничения передвижения только по игровому полю
    //public bool IsPointInPolygon(Vector2 point)
    //{
    //    int polygonLength = polygon.Length, i = 0;
    //    bool inside = false;
    //    // x, y for tested point.
    //    float pointX = point.x, pointY = point.y;
    //    // start / end point for the current polygon segment.
    //    float startX, startY, endX, endY;
    //    Vector2 endPoint = polygon[polygonLength - 1];
    //    endX = endPoint.x;
    //    endY = endPoint.y;
    //    while (i < polygonLength)
    //    {
    //        startX = endX; startY = endY;
    //        endPoint = polygon[i++];
    //        endX = endPoint.x; endY = endPoint.y;
    //        //
    //        inside ^= (endY > pointY ^ startY > pointY) /* ? pointY inside [startY;endY] segment ? */
    //                  && /* if so, test if it is under the segment */
    //                  ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
    //    }
    //    return inside;
    //}
    public bool ContainsPoint(Vector2 p)
    {
        var j = polygon.Length - 1;
        var inside = false;
        for (int i = 0; i < polygon.Length; j = i++)
        {
            var pi = polygon[i];
            var pj = polygon[j];
            if (((pi.y <= p.y && p.y < pj.y) || (pj.y <= p.y && p.y < pi.y)) &&
                (p.x < (pj.x - pi.x) * (p.y - pi.y) / (pj.y - pi.y) + pi.x))
                inside = !inside;
        }
        return inside;
    }

}
