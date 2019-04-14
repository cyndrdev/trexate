using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelManager : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float _time = 0;

    public float GetCurrentTimescale()
        => _time;

    public int GetCurrentDate()
        => GetDate(_time);

    public int GetDate(float t)
    {
        int diff = GameConstants.TimeTravelEnd - GameConstants.TimeTravelStart;
        float m = Mathf.Pow(t, GameConstants.TimeTravelExponent);
        return GameConstants.TimeTravelStart + Mathf.RoundToInt(diff * m);
    }

    void Update()
    {
        _time += Time.deltaTime * 0.01f;
        Game.Instance.GlobalState.Counters["date"] = GetCurrentDate();
    }
}
