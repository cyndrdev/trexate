using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramerateCounter : MonoBehaviour
{
    private Text _counter;
    private float _framerate;
    [SerializeField]
    [Range(0f, 1f)]
    private float _smoothing;

    void Start()
    {
        _counter = this.GetComponent<Text>();

        if (_counter == null)
            throw new System.Exception();
    }

    void Update()
    {
        float currentFramerate = 1f / Time.deltaTime;
        _framerate = Mathf.Lerp(_framerate, currentFramerate, _smoothing);
        _counter.text = Mathf.RoundToInt(_framerate).ToString();
    }
}
