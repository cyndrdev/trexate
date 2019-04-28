using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class HitboxViewer : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _playerGraphics;
    [SerializeField]
    private SpriteRenderer _hitboxGraphics;

    private bool _isSlowmo;

    private float _playerAlpha;
    private float PlayerTargetAlpha
        => (_isSlowmo && !Game.Instance.IsPaused) ? 0.5f : 1f;

    private float _hitboxAlpha;
    private float HitboxTargetAlpha
        => (_isSlowmo && !Game.Instance.IsPaused) ? 1f : 0f;

    private void Start()
    {
        Game.GetPersistentComponent<InputManager>()
            .Secondary
            .AddListener((b) => _isSlowmo = b);
    }

    void Update()
    {
        _playerAlpha = Mathf.Lerp(_playerAlpha, PlayerTargetAlpha, GameConstants.HitboxFadeAmt * Time.unscaledDeltaTime);
        _hitboxAlpha = Mathf.Lerp(_hitboxAlpha, HitboxTargetAlpha, GameConstants.HitboxFadeAmt * Time.unscaledDeltaTime);

        _playerGraphics.color = _playerGraphics.color.WithAlpha(_playerAlpha);
        _hitboxGraphics.color = _hitboxGraphics.color.WithAlpha(_hitboxAlpha);
    }

    private void LateUpdate()
    {
        _hitboxGraphics.gameObject.transform.rotation = Quaternion.identity;
    }
}
