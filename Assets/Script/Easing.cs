using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Easing
{
    static public float EaseOutExpo(float t)
    {
        return t == 1 ? 1 : 1 - Mathf.Pow(2, -10 * t);
    }

    static public float EaseInExpo(float t)
    {
        return t == 0 ? 0 : Mathf.Pow(2, 10 * t - 10);
    }
}
