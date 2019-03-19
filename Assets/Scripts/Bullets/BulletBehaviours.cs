using System;
using UnityEngine;

public static class BulletBehaviours
{
    public static Func<float, Vector2> sinusoidal =
        t => new Vector2(Mathf.Sin(t), t);
}
