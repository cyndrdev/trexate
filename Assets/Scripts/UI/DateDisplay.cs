using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class DateDisplay : MonoBehaviour
{
    private Text _display;
    private int _date;

#pragma warning disable 0649
    [SerializeField]
    private bool _showCurrentDate;
#pragma warning restore 0649

    [SerializeField]
    [Range(0, 1)]
    private float _time;

    void Start()
    {
        _display = this.GetComponent<Text>();

        if (_display == null)
            throw new System.Exception();
    }

    void Update()
    {
        if (_showCurrentDate)
            _time = Game.Instance.TimeTravelManager.GetCurrentTimescale();

        _date = Game.Instance.TimeTravelManager.GetDate(_time);

        _display.text = _date.ToYearString();
    }
}
