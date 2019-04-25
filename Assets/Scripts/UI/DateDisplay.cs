using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class DateDisplay : MonoBehaviour
{
    private TimeTravelManager _timeTravelManager;
    private Text _display;
    private int _date;

#pragma warning disable 0649
    [SerializeField]
    private bool _showCurrentDate;

    [SerializeField]
    private bool _showCurrentTarget;
#pragma warning restore 0649

    [SerializeField]
    [Range(0, 1)]
    private float _time;

    void Start()
    {
        _display = this.GetComponent<Text>();
        _timeTravelManager = Game.GetPersistentComponent<TimeTravelManager>();

        if (_display == null)
            throw new System.Exception();
    }

    void Update()
    {
        if (_showCurrentDate)
            _time = _timeTravelManager.GetCurrentTimescale();

        else if (_showCurrentTarget)
            _time = _timeTravelManager.Target;

        _date = _timeTravelManager.GetDate(_time);

        _display.text = _date.ToYearString();
    }
}
