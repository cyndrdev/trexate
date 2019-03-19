using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletBehaviours
{
    public static Dictionary<string, Func<float, Vector2>> Behaviours
        = new Dictionary<string, Func<float, Vector2>>
    {
        { "sinusoidal", t => new Vector2(Mathf.Sin(t), t) }
    };
}
