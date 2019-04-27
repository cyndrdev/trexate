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
    private Image _subBar;

    private float _barSize;

    private IScaleProvider _source;
    float _scale;

    // Start is called before the first frame update
    void Start()
    {
        _source = this.GetComponent<IScaleProvider>();
        if (_source == null)
            throw new System.Exception("[BarRenderer]: no IScaleProvider instance on bar.");

        _barSize = _bar.rectTransform.rect.width;
        _barSize -= GameConstants.BarMargin;
    }

    // Update is called once per frame
    void Update()
    {
        if (_source == null)
            return;

        SetBarValue(_bar, _barSize, _source.GetValue());

        if (_subBar != null)
            SetBarValue(_subBar, _barSize, _source.GetSubValue());
    }

    public void SetBarValue(Image bar, float barSize, float value)
    {
        bar.color = bar.color.WithAlpha((value <= 0f) ? 0f : 1f);
        bar.rectTransform.offsetMax = new Vector2((value - 1f) * barSize, 0);
    }
}
