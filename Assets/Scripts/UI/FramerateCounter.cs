using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class FramerateCounter : MonoBehaviour
{
    private Text _counter;
    private float _framerate;

    [SerializeField]
    private int _historySize;

    private float[] _history;

    private int position;

    void Start()
    {
        _counter = this.GetComponent<Text>();

        if (_counter == null)
            throw new System.Exception();

        _history = new float[_historySize];
    }

    void Update()
    {
        _history[position++] = Time.deltaTime;

        while (position >= _historySize)
            position -= _historySize;

        int frameRate = (int)(1f / _history.Average());
        _counter.text = frameRate.ToString();
    }
}
