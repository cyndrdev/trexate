using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalState : MonoBehaviour
{
    public Dictionary<string, int> Counters;
    public int Score { get => Counters["score"]; }

    void Awake()
    {
        Counters = new Dictionary<string, int>();
        Counters.Add("score", 0);
        Counters.Add("date", 0);
    }
}
