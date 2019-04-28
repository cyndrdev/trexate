using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Extensions;

public class DeathScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject _fadeObject;
    private ScreenFader _fader;

    [SerializeField]
    private Text _deathText;
    [SerializeField]
    private Text _restartText;

    private bool _canRestart = false;

    public void StartDeathFade()
    {
        Time.timeScale = 0f;

        Game.GetPersistentComponent<InputManager>()
            .Tertiary
            .AddListener((b) => { if (b && _canRestart) ReloadScene(); });

        _deathText.enabled = true;
        _restartText.enabled = true;
        _fader = _fadeObject.GetComponent<ScreenFader>();
        StartCoroutine(DoDeathFade());
    }

    private IEnumerator DoDeathFade()
    {
        float t = 0f;
        bool hasFadedBg = false;
        float componentDuration = GameConstants.DeathFadeDuration / 1.5f;

        while (t < 1f)
        {
            _deathText.color = _deathText.color.WithAlpha(t);
            yield return new WaitForEndOfFrame();
            t += Time.unscaledDeltaTime / componentDuration;
            if (t >= 0.5f && !hasFadedBg)
            {
                _fader.FadeTo(1f, componentDuration);
                hasFadedBg = true;
            }
        }

        _canRestart = true;
        yield return new WaitForSecondsRealtime(componentDuration / 2f);

        _restartText.color = _restartText.color.WithAlpha(1f);
        while (true)
        {
            yield return new WaitForSecondsRealtime(GameConstants.DeathBlinkRate);
            _restartText.color = _restartText.color.WithAlpha(1f - _restartText.color.a);
        }
    }

    private void ReloadScene()
    {
        Debug.Log("restart pls");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
