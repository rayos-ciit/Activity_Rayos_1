using UnityEngine;

public static class EquationsUtility
{
    //quadratic
    public static float EaseOutQuad(float t)
    {
        return 1f - (1f - t) * (1f - t);
    }
    
    //quadratic Bézier
    public static Vector3 QuadraticFast(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1f - t;
        return (u * u * p0) + (2f * u * t * p1) + (t * t * p2);
    }

    //cubic Bézier
    public static Vector3 CubicFast(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float u = 1f - t;
        return (u * u * u * p0) + (3 * u * u * t * p1) + (3 * u * t * t * p2) + (t * t * t * p3);
    }

}