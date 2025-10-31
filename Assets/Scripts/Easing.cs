using System;
using System.Collections.Generic;
using UnityEngine;

public static class Easing
{
    public enum Type
    {
        Linear,
        Quad,
        Cubic,
        Expo,
        Circ,
        Back,
        Elastic,
        Sine
    }

    private static readonly Dictionary<Type, Func<float, float>> _easingMap = new()
    {
        { Type.Linear, Linear },
        { Type.Quad, QuadEaseInOut },
        { Type.Cubic, CubicEaseInOut },
        { Type.Expo, ExpoEaseInOut },
        { Type.Circ, CircEaseInOut },
        { Type.Back, t => BackEaseInOut(t, 1.70158f) },
        { Type.Elastic, ElasticEaseInOut },
        { Type.Sine, SineEaseInOut }
    };

    public static float Apply(float t, Type type)
    {
        if (_easingMap.TryGetValue(type, out var func))
            return func(Mathf.Clamp01(t)); // защита от выхода за [0..1]
        
        Debug.LogWarning($"Easing type {type} not found, using Linear.");
        return Linear(t);
    }

    private static float Linear(float t) => t;

    private static float QuadEaseInOut(float t)
        => t < 0.5f ? 2 * t * t : 1 - Mathf.Pow(-2 * t + 2, 2) / 2;

    private static float CubicEaseInOut(float t)
        => t < 0.5f ? 4 * t * t * t : 1 - Mathf.Pow(-2 * t + 2, 3) / 2;

    private static float ExpoEaseInOut(float t)
        => t == 0 ? 0 : t == 1 ? 1 :
           t < 0.5f ? Mathf.Pow(2, 20 * t - 10) / 2
                    : (2 - Mathf.Pow(2, -20 * t + 10)) / 2;

    private static float CircEaseInOut(float t)
        => t < 0.5f ? (1 - Mathf.Sqrt(1 - Mathf.Pow(2 * t, 2))) / 2
                    : (Mathf.Sqrt(1 - Mathf.Pow(-2 * t + 2, 2)) + 1) / 2;

    private static float BackEaseInOut(float t, float s)
        => t < 0.5f
            ? (Mathf.Pow(2 * t, 2) * ((s + 1) * 2 * t - s)) / 2
            : (Mathf.Pow(2 * t - 2, 2) * ((s + 1) * (t * 2 - 2) + s) + 2) / 2;

    private static float ElasticEaseInOut(float t)
    {
        const float c5 = (2 * Mathf.PI) / 4.5f;
        if (t == 0) return 0;
        if (t == 1) return 1;
        return t < 0.5f
            ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2
            : (Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * c5)) / 2 + 1;
    }

    private static float SineEaseInOut(float t)
        => -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
}

