using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class ScreenFader : MonoBehaviour
{
    private float _level = 1f;
    private float _startLevel;
    private Color _baseColor;
    private Image _image;
    private IEnumerator _fadeCoroutine;

    void Awake()
    {
        _image = this.GetComponent<Image>();
        _baseColor = _image.color;
        SetLevel(_level);
    }

    void SetLevel(float f)
        => _image.color = _baseColor.WithAlpha(f);

    public void FadeTo(float target, float time)
    {
        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = DoFade(target, time);
        StartCoroutine(_fadeCoroutine);
    }

    public void FadeToFrom(float start, float target, float time)
    {
        SetLevel(start);
        FadeTo(target, time);
    }

    private IEnumerator DoFade(float target, float time) {
        _startLevel = _level;
        float t = 0f;
        float diff = target - _level;

        while (t < 1f)
        {
            SetLevel(_startLevel + diff * t);
            yield return new WaitForEndOfFrame();
            t += Time.unscaledDeltaTime / time;
            _level = _startLevel + diff * t;
        }
            SetLevel(target);
    }
}
