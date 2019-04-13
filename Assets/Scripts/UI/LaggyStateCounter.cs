using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class LaggyStateCounter : MonoBehaviour
{
    [SerializeField]
    private string _key;

    [SerializeField]
    private bool _isPlayerPref;

    private int _value;
    private int _shownValue = 0;

    private Text _counter;

    void Start()
    {
        _counter = this.GetComponent<Text>();
        if (_counter == null)
            throw new System.Exception();
        StartCoroutine(KeepUp());
    }

    void DoStep()
    {
        // grab our value
        if (_isPlayerPref)
            _value = PlayerPrefs.GetInt(_key);
        else
            _value = Game.Instance.GlobalState.Counters[_key];

        // nothing to count up to
        if (_value <= _shownValue)
            return;

        // get our step
        int step;
        int diff = _value - _shownValue;
        float exp = Mathf.Log10(diff) - 0.4f;

        step = 10.Pow((int)exp);

        float subStep = exp - Mathf.Floor(exp);
        if (subStep >= 0.7f)
            step *= 5;
        else if (subStep >= 0.3f)
            step *= 2;

        // round to nearest step
        _shownValue += step;
        _shownValue = (int)(_shownValue / step) * step;

        if (_shownValue > _value)
            _shownValue = _value;

        // TODO: actually be laggy
        //_shownValue = _value;

        // update our text
        _counter.text = _shownValue.ToString();
    }

    public IEnumerator KeepUp()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(GameConstants.CounterRefreshRate);
            DoStep();
        }
    }
}
