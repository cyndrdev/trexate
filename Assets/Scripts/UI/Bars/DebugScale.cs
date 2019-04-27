using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScale : MonoBehaviour, IScaleProvider
{
    [Range(0f, 1f)]
    public float value = 0.5f;
    [Range(0f, 1f)]
    public float subValue = 0.5f;

    public float GetValue() => value;
    public float GetSubValue() => subValue;
}
