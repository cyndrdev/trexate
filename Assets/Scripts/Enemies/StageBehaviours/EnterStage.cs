﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterStage : IEnemyState
{
    private GameObject _gameObject;
    private EnemyController _controller;
    private float _t = 0f;
    private static float _speedMulti = 7.5f;
    private static float _stageTime = 0.7f;

    /* === interface methods === */
    public void Start(GameObject gameObject, EnemyController controller)
    {
        _gameObject = gameObject;
        _controller = controller;

        _controller.IsInvulnerable = true;
    }

    public void Update()
    {
        _t += Time.deltaTime;
        _gameObject.transform.Translate(Vector2.down * Time.deltaTime * _speedMulti);

        if (_t >= _stageTime)
            _controller.Damage(1, true);
    }

    public void End()
    {
        _controller.IsInvulnerable = false;
    }
}
