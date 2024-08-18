using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Easing
{
    public enum EasingType
    {
        Linear,
        QuadraticIn,
        QuadraticOut,
        QuadraticInOut,
        CubicIn,
        CubicOut,
        CubicInOut,
        QuarticIn,
        QuarticOut,
        QuarticInOut,
        QuinticIn,
        QuinticOut,
        QuinticInOut,
        SinusoidalIn,
        SinusoidalOut,
        SinusoidalInOut,
        CircularIn,
        CircularOut,
        CircularInOut,
        ElasticIn,
        ElasticOut,
        ElasticInOut,
        BackIn,
        BackOut,
        BackInOut,
        BounceIn,
        BounceOut,
        BounceInOut,
        ExponentialIn,
        ExponentialOut,
        ExponentialInOut
    }
    
    public static Func<float, float> GetEasingFunction(EasingType t)
    {
        switch (t)
        {
            case EasingType.Linear: return EaseLinear;
            case EasingType.QuadraticIn: return EaseInQuadratic;
            case EasingType.QuadraticOut: return EaseOutQuadratic;
            case EasingType.QuadraticInOut: return EaseInOutQuadratic;
            case EasingType.CubicIn: return EaseInCubic;
            case EasingType.CubicOut: return EaseOutCubic;
            case EasingType.CubicInOut: return EaseInOutCubic;
            case EasingType.QuarticIn: return EaseInQuartic;
            case EasingType.QuarticOut: return EaseOutQuartic;
            case EasingType.QuarticInOut: return EaseInOutQuartic;
            case EasingType.QuinticIn: return EaseInQuintic;
            case EasingType.QuinticOut: return EaseOutQuintic;
            case EasingType.QuinticInOut: return EaseInOutQuintic;
            case EasingType.SinusoidalIn: return EaseInSinusoidal;
            case EasingType.SinusoidalOut: return EaseOutSinusoidal;
            case EasingType.SinusoidalInOut: return EaseInOutSinusoidal;
            case EasingType.CircularIn: return EaseInCircular;
            case EasingType.CircularOut: return EaseOutCircular;
            case EasingType.CircularInOut: return EaseInOutCircular;
            case EasingType.ElasticIn: return EaseInElastic;
            case EasingType.ElasticOut: return EaseOutElastic;
            case EasingType.ElasticInOut: return EaseInOutElastic;
            case EasingType.BackIn: return EaseInBack;
            case EasingType.BackOut: return EaseOutBack;
            case EasingType.BackInOut: return EaseInOutBack;
            case EasingType.BounceIn: return EaseInBounce;
            case EasingType.BounceOut: return EaseOutBounce;
            case EasingType.BounceInOut: return EaseInOutBounce;
            case EasingType.ExponentialIn: return EaseInExpo;
            case EasingType.ExponentialOut: return EaseOutExpo;
            case EasingType.ExponentialInOut: return EaseInOutExpo;
            default: return EaseLinear;
        }
    }

    public static float s = 1.70158f, s2 = 2.5949095f;

    // Cubic Bezier
    // https://cubic-bezier.com/
    public static float CubicBezier(float t, float p0, float p1, float p2, float p3)
    {
        return Mathf.Pow(1 - t, 3) * p0 + 3 * Mathf.Pow(1 - t, 2) * t * p1 + 3 * (1 - t) * Mathf.Pow(t, 2) * p2 + Mathf.Pow(t, 3) * p3;
    }

    // Exponencial
    public static float EaseInExpo(float t) { return t == 0f ? 0f : Mathf.Pow(1024f, t - 1f); }
    public static float EaseOutExpo(float t) { return t == 1f ? 1f : 1f - Mathf.Pow(2f, - 10f * t); }
    public static float EaseInOutExpo(float t) 
    {
        if (t == 0f) return 0f;
        if (t == 1f) return 1f;
        if ((t *= 2f) < 1f) return 0.5f * Mathf.Pow(1024f, t - 1f);
        return 0.5f * (- Mathf.Pow(2f, - 10f * (t - 1f)) + 2f); 
    }

    // Bounce
    public static float EaseOutBounce(float t)
    {
        if (t < (1f / 2.75f)) return 7.5625f * t * t;				
        else if (t < (2f / 2.75f)) return 7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f;
        else if (t < (2.5f / 2.75f)) return 7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f;
        else return 7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f;
    }
    public static float EaseInBounce(float t) { return 1f - EaseOutBounce(1f - t); }
    public static float EaseInOutBounce(float t) 
    {
        if (t < 0.5f) return EaseInBounce(t * 2f) * 0.5f;
        return EaseOutBounce(t * 2f - 1f) * 0.5f + 0.5f;
    }

    // Back
    public static float EaseOutBack(float t) { return (t -= 1f) * t * ((s + 1f) * t + s) + 1f; }
    public static float EaseInBack(float t) { return t * t * ((s + 1f) * t - s); }
    public static float EaseInOutBack(float t) 
    { 
        if ((t *= 2f) < 1f) return 0.5f * (t * t * ((s2 + 1f) * t - s2));
        return 0.5f * ((t -= 2f) * t * ((s2 + 1f) * t + s2) + 2f);
    }

    // Elastic
    public static float EaseOutElastic(float t) 
    {
        if (t == 0) return 0;
        if (t == 1) return 1;
        return Mathf.Pow(2f, - 10f * t) * Mathf.Sin((t - 0.1f) * (2f * Mathf.PI) / 0.4f) + 1f;
    }
    public static float EaseInElastic(float t) 
    { 
        if (t == 0) return 0;
        if (t == 1) return 1;
        return - Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t - 0.1f) * (2f * Mathf.PI) / 0.4f);
    }
    public static float EaseInOutElastic(float t) 
    { 
        if ((t *= 2f) < 1f) return - 0.5f * Mathf.Pow(2f, 10f * (t -= 1f)) * Mathf.Sin((t - 0.1f) * (2f * Mathf.PI) / 0.4f);
        return Mathf.Pow(2f, - 10f * (t -= 1f)) * Mathf.Sin((t - 0.1f) * (2f * Mathf.PI) / 0.4f) * 0.5f + 1f;
    }

    // Circular
    public static float EaseOutCircular(float t) { return Mathf.Sqrt(1f - ((t -= 1f) * t)); }
    public static float EaseInCircular(float t) { return 1f - Mathf.Sqrt(1f - t * t); }
    public static float EaseInOutCircular(float t) 
    { 
        if ((t *= 2f) < 1f) return - 0.5f * (Mathf.Sqrt(1f - t * t) - 1);
        return 0.5f * (Mathf.Sqrt(1f - (t -= 2f) * t) + 1f);
    }
    
    // Sinusoidal
    public static float EaseOutSinusoidal(float t) { return Mathf.Sin(t * Mathf.PI / 2f); }
    public static float EaseInSinusoidal(float t) { return 1f - Mathf.Cos(t * Mathf.PI / 2f); }
    public static float EaseInOutSinusoidal(float t) { return 0.5f * (1f - Mathf.Cos(Mathf.PI * t)); }
    
    // Quintic
    public static float EaseOutQuintic(float t) { return 1f + ((t -= 1f) * t * t * t * t); }
    public static float EaseInQuintic(float t) { return t * t * t * t * t; }
    public static float EaseInOutQuintic(float t) 
    { 
        if ((t *= 2f) < 1f) return 0.5f * t * t * t * t * t;
        return 0.5f * ((t -= 2f) * t * t * t * t + 2f);
    }
    
    // Quartic
    public static float EaseOutQuartic(float t) { return 1f - ((t -= 1f) * t * t * t); }
    public static float EaseInQuartic(float t) { return t * t * t * t; }
    public static float EaseInOutQuartic(float t) 
    { 
        if ((t *= 2f) < 1f) return 0.5f * t * t * t * t;
        return - 0.5f * ((t -= 2f) * t * t * t - 2f);
    }

    // Cubic
    public static float EaseOutCubic(float t) { return 1f + ((t -= 1f) * t * t); }
    public static float EaseInCubic(float t) { return t * t * t; }
    public static float EaseInOutCubic(float t) 
    { 
        if ((t *= 2f) < 1f) return 0.5f * t * t * t;
        return 0.5f * ((t -= 2f) * t * t + 2f);
    }

    // Quadratic
    public static float EaseOutQuadratic(float t) { return t * (2f - t); }
    public static float EaseInQuadratic(float t) { return t * t; }
    public static float EaseInOutQuadratic(float t) 
    { 
        if ((t *= 2f) < 1f) return 0.5f * t * t;
        return - 0.5f * ((t -= 1f) * (t - 2f) - 1f);
    }

    // Linear
    public static float EaseLinear(float t) { return t; }
}
