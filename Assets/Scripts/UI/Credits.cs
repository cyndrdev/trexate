using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;

public class Credits : MonoBehaviour
{
    public GameObject _background;
    public GameObject _endScreen;
    public RectTransform _creditsScroll;
    public string _creditsMusic;

    private Image _bgImage;

    private bool _stopScroll = false;
    private bool _disabled = true;

    private Vector2 CreditsPos
    {
        get => new Vector2(0, _creditsY);
    }

    private float _creditsY;
    private float _creditsBounds;

    public void Begin()
    {
        _creditsBounds = (_creditsScroll.sizeDelta.y + 1080) / 2;
        _creditsY = -(_creditsBounds + GameConstants.CreditsTopMargin);
        _creditsScroll.anchoredPosition = CreditsPos;
        Time.timeScale = 0;
        Game.GetPersistentComponent<SoundEngine>().PlayMusic(_creditsMusic);
        _disabled = false;
        StartCoroutine(FadeInCredits());
    }

    // Update is called once per frame
    void Update()
    {
        if (_stopScroll || _disabled)
            return;

        _creditsY += Time.unscaledDeltaTime * GameConstants.CreditsScrollSpeed;
        _creditsScroll.anchoredPosition = CreditsPos;

        if (_creditsY >= _creditsBounds)
        {
            StartCoroutine(FadeInEndText());
            _stopScroll = true;
        }
    }

    private IEnumerator FadeInEndText()
    {
        float t = 0f;
        _endScreen.SetActive(true);
        while (t < 1f)
        {
            foreach (var text in _endScreen.GetComponentsInChildren<Text>())
            {
                text.color = text.color.WithAlpha(t);
            }

            yield return new WaitForEndOfFrame();
            t += Time.unscaledDeltaTime / GameConstants.CreditsEndTextFadeSpeed;
        }
    }

    private IEnumerator FadeInCredits()
    {
        _background.SetActive(true);

        var image = _background.GetComponent<Image>();
        float t = 0f;

        while (t < 1f)
        {
            image.color = image.color.WithAlpha(t);
            yield return new WaitForEndOfFrame();
            t += Time.unscaledDeltaTime / GameConstants.CreditsFadeInSpeed;
        }
    }
}
