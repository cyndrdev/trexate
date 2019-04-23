using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class NewAreaTitle : MonoBehaviour
{
    private float _level = 0f;
    private Text _text;
    private Color _baseColor;

    void SetFadeLevel(float f)
        => _text.color = _baseColor.WithAlpha(_level);

    private void Start()
    {
        _text = GetComponent<Text>();
        _baseColor = _text.color;
    }

    public float DisplayText(string text)
    {
        _text.text = text;
        StartCoroutine(DoTextDisplay());
        return (
            GameConstants.NewAreaTextFadeInDuration +
            GameConstants.NewAreaTextHoldDuration +
            GameConstants.NewAreaTextFadeOutDuration
        );
    }

    private IEnumerator DoTextDisplay()
    {
        _level = 0f;
        SetFadeLevel(_level);
        while (_level < 1f)
        {
            yield return new WaitForEndOfFrame();
            _level += Time.deltaTime / GameConstants.NewAreaTextFadeInDuration;
            SetFadeLevel(_level);
        }
        yield return new WaitForSeconds(GameConstants.NewAreaTextHoldDuration);
        while (_level > 0f)
        {
            yield return new WaitForEndOfFrame();
            _level -= Time.deltaTime / GameConstants.NewAreaTextFadeOutDuration;
            SetFadeLevel(_level);
        }
    }
}
