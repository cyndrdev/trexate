using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class BarRenderer : MonoBehaviour
{
    [SerializeField]
    private Image _bar;
    [SerializeField]
    private Image _laggyBar;

    private float _barSize;

    private bool _useLaggyBar;

    private IScaleProvider _source;
    float _scale;

    // Start is called before the first frame update
    void Start()
    {
        _source = this.GetComponent<IScaleProvider>();
        if (_source == null)
            throw new System.Exception("[BarRenderer]: no IScaleProvider instance on bar.");

        _useLaggyBar = (_laggyBar != null);

        _barSize = _bar.rectTransform.rect.width;
        _barSize -= GameConstants.BarMargin;
    }

    // Update is called once per frame
    void Update()
    {
        if (_source == null)
            return;

        _scale = _source.GetValue();

        _bar.color = _bar.color.WithAlpha((_scale <= 0f) ? 0f : 1f);
        var size = _bar.rectTransform.sizeDelta;
        _bar.rectTransform.offsetMax = new Vector2((_scale - 1f) * _barSize, 0);

        Debug.Log(size);
    }
}
