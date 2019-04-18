using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeSmall : IEnemyState
{
    private EnemyController _controller;
    private GameObject _gameObject;
    private IEnumerator _explosion;
    private static int _shots = 16;
    private static int _shotsIncrease = 8;
    private static int _rounds = 3;
    private static float _roundDelay = 0.25f;
    private static string _bulletName = BulletTypes.ExplosionDebris;

    /* === enemy state methods === */
    public void Start(GameObject gameObject, EnemyController controller)
    {
        _controller = controller;
        _gameObject = gameObject;

        _explosion = Explode();
        _controller.StartCoroutine(_explosion);

        _controller.IsInvulnerable = true;
    }

    public void End()
    {
        _controller.StopCoroutine(_explosion);
    }

    public void Update()
    {

    }

    /* === helper methods === */
    private void FireAround()
    {
        for (int i = 0; i < _shots; i++)
        {
            float angle = (360f / _shots) * i;
            Fire(angle);
        }
        //_shots = Mathf.RoundToInt(_shots * 1.5f);
        _shots += 4;
    }

    private void Fire(float angle)
    {
        _gameObject.Shoot(_bulletName, new Vector2(0, 0), angle);
    }

    private IEnumerator Explode()
    {
        //_controller.IsVisible = false;
        for (int i = 0; i < _rounds - 1; i++)
        {
            FireAround();
            yield return new WaitForSeconds(_roundDelay);
        }
        FireAround();
        _controller.Die(true);
    }
}
