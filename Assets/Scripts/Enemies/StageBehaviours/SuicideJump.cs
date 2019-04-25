using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class SuicideJump : IEnemyState
{
    private GameObject _gameObject;
    private EnemyController _controller;
    private static float _jumpDuration = 0.2f;
    private static float _jumpSpeed = 125f;
    private static float _proxyTrigger = 0.3f;
    private static float _exp = 1.6f;
    private static int _explosionShots = 8;
    private static string _bulletName = BulletTypes.ExplosionDebris;

    private Vector2 _direction;
    private float _t;

    /* === interface methods === */
    public void Start(GameObject gameObject, EnemyController controller)
    {
        _gameObject = gameObject;
        _controller = controller;

        _direction = (Game.Instance.PlayerPosition - _gameObject.transform.position).normalized;
    }

    public void Update()
    {
        _t += Time.deltaTime;

        float expMultiplier = Mathf.Pow(_t / _jumpDuration, _exp);
        _gameObject.transform.Translate(_direction * Time.deltaTime * expMultiplier * _jumpSpeed);

        // lifetime check
        if (_t >= _jumpDuration)
            _controller.Die(true);

        // proximity check
        Vector2 distance = Game.Instance.PlayerPosition - _gameObject.transform.position;
        if (distance.magnitude <= _proxyTrigger)
            _controller.Die(true);
    }

    public void End()
    {
        for (int i = 0; i < _explosionShots; i++)
        {
            float angle = (360f / _explosionShots) * i;
            _gameObject.Shoot(_bulletName, Vector2.zero, angle);
        }
    }
}
