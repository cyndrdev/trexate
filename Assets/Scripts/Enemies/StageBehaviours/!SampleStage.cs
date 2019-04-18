using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleStage : IEnemyState
{
    private GameObject _gameObject;
    private EnemyController _controller;

    /* === interface methods === */
    public void Start(GameObject gameObject, EnemyController controller)
    {
        _gameObject = gameObject;
        _controller = controller;
    }

    public void Update()
    {

    }

    public void End()
    {

    }
}
