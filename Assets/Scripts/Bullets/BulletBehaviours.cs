using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public static class BulletBehaviours
{
    public static Dictionary<string, Func<float, Vector2>> Behaviours
        = new Dictionary<string, Func<float, Vector2>>
        {
            { "linear", t => new Vector2(0, t) },
            { "sinusoidal", t => new Vector2(Mathf.Sin(t), t) },
            { "progressive", t => new Vector2(0, t + Mathf.Sin(t)) },
            { "parametric", t => new Vector2(Mathf.Sin(t), Mathf.Sin(t / 2)) }
        };
}
