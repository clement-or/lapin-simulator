using UnityEngine;

public static class Helpers
{
    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}

/// <summary>
/// Classe décrivant un minimum et un maximum
/// </summary>
public class Range
{
    public float min;
    public float max;
    
    public Range(float min, float max)
    {
        this.min = min;
        this.max = max;
    }
}