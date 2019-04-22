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

    void Start()
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
        float diff = target - _level;
        while (true)
        {
            yield return new WaitForEndOfFrame();
            _level += diff * (Time.deltaTime / time);

            if ((diff < 0f && _level < target) || (diff > 0f && _level > target))
            {
                _level = target;
                yield return null;
            }

            SetLevel(_level);
        }
    }
}
