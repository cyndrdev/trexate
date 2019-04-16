using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FramerateCounter : MonoBehaviour
{
    private Text _counter;
    private float _framerate;

#pragma warning disable 0649
    [SerializeField]
    [Range(0f, 1f)]
    private float _smoothing;
#pragma warning restore 0649

    void Start()
    {
        _counter = this.GetComponent<Text>();

        if (_counter == null)
            throw new System.Exception();

        // don't show counter while it's still gathering data
        // for the first second or so
        _counter.enabled = false;
        StartCoroutine(ShowCounter());
    }

    void Update()
    {
        float currentFramerate = 1f / Time.unscaledDeltaTime;
        _framerate = Mathf.Lerp(_framerate, currentFramerate, _smoothing);
        _counter.text = Mathf.RoundToInt(_framerate).ToString();
    }

    private IEnumerator ShowCounter()
    {
        yield return new WaitForSecondsRealtime(GameConstants.FramerateDisplayDelay);
        _counter.enabled = true;
    }
}
