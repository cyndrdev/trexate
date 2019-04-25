using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTravelManager : MonoBehaviour
{
    [SerializeField]
    [Range(0, 1)]
    private float _time = 0;
    private GlobalState _globalState;

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

    public float GetT(int date)
    {
        float m = (
            (float)(GameConstants.TimeTravelStart - date) /
            (GameConstants.TimeTravelStart - GameConstants.TimeTravelEnd)
        );

        return Mathf.Pow(m, 1f / GameConstants.TimeTravelExponent);
    }

    private void Start()
    {
        _globalState = Game.GetPersistentComponent<GlobalState>();
    }

    void Update()
    {
        /*
        _time += Time.deltaTime / GameConstants.TimeTravelDuration;

        if (_time > 1f)
            _time = 1f;
        */

        _globalState.Counters["date"] = GetCurrentDate();
    }

    public void DoJump(float target, float duration)
        => StartCoroutine(JumpTo(target, duration));

    public void DoJump(int target, float duration)
        => DoJump(GetT(target), duration);

    private IEnumerator JumpTo(float target, float duration)
    {
        float t = 0;
        float start = _time;
        float dist = target - start;

        while (t <= duration)
        {
            yield return new WaitForEndOfFrame();
            t += Time.deltaTime;
            _time = start + dist * (t / duration);
        }
        _time = target;
    }
}
