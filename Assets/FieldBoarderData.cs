using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FieldBoarderData : MonoBehaviour
{
    [SerializeField]
    private CurvedLinePoint[] points;
    private Vector2[] polygon;

    Vector2 cenerMass;

    public Vector2 CenerMass => cenerMass;

    void Awake()
    {
        points = GetComponentsInChildren<CurvedLinePoint>();
        polygon = new Vector2[points.Length];
        for (var i = 0; i < points.Length; i++)
            polygon[i] = new Vector2(points[i].transform.position.x, points[i].transform.position.z);

        CenterMass();
    }

    private void CenterMass()
    {

        if (polygon.Length > 2)
        {
            float centerX = polygon.Select(x => x.x * 1).Sum() / polygon.Length;
            float centerY = polygon.Select(x => x.y * 1).Sum() / polygon.Length;

            cenerMass = new Vector2(centerX, centerY);
        }
    }
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
