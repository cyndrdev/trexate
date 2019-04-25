using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpewRockets : IEnemyState
{
    private float _shotsPerSecond = 0.5f;
    private IEnumerator _behaviourCoroutine;
    private string _bulletName = BulletTypes.EnemyRocket;
    private GameObject _gameObject;
    private MonoBehaviour _controller;

    public void Start(GameObject gameObject, EnemyController controller)
    {
        _controller = controller;
        _gameObject = gameObject;

        _behaviourCoroutine = FireRockets();
        _controller.StartCoroutine(_behaviourCoroutine);
    }

    public void End()
    {
        _controller.StopCoroutine(_behaviourCoroutine);
    }

    public void Update()
    {
    }

    void Fire()
    {
        _gameObject.Shoot(_bulletName, new Vector2(0,0), Random.Range(-Mathf.PI, Mathf.PI));
    }

    private IEnumerator FireRockets()
    {
        while(true)
        {
            Fire();
            yield return new WaitForSeconds(1f / _shotsPerSecond);
        }
    }
}
