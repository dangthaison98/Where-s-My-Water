using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Caculate
{
    public static Vector2 GetIntersectionPoint(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
        float d1 = Vector3.Cross(p1 - p3, p4 - p3).z;
        float d2 = Vector3.Cross(p2 - p3, p4 - p3).z;
        if (d1 - d2 == 0) return Vector2.negativeInfinity;
        return (d1 * p2 - d2 * p1) / (d1 - d2);
    }

    public static Vector2 FindPointInCircle(Vector2 center, float radius, float angle)
    {
        Vector2 pos = new Vector2();
        pos.x = center.x + (radius * Mathf.Cos(angle / (180f / Mathf.PI)));
        pos.y = center.y + (radius * Mathf.Sin(angle / (180f / Mathf.PI)));
        return pos;
    }
}
