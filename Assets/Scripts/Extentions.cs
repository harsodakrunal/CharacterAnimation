using System;
using UnityEngine;

public static class Extentions
{
    public static float Length(this Vector3 pos)
    {
        return (float)Math.Sqrt(pos.x * pos.x + pos.y * pos.y + pos.z * pos.z);
    }

    public static Vector3 ConvertToLocal(this Vector3 position, Transform transform)
    {
        return transform.InverseTransformPoint(position);
    }
}
