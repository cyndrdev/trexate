using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontShotgun : IEnemyState
{
    private GameObject _gameObject;
    private EnemyController _controller;
    private static int[] _shotgunSize = { 5, 9, 5, 9, 13 };
    private static float _shotgunSpread = 180f;
    private static float _shotDelay = 0.3f;
    private static float _groupDelay = 1f;
    private static string _bulletName = BulletTypes.Simple;
    private IEnumerator _fireCoroutine;

    /* === interface methods === */
    public void Start(GameObject gameObject, EnemyController controller)
    {
        _gameObject = gameObject;
        _controller = controller;
        _fireCoroutine = FireShotguns();
        _controller.StartCoroutine(_fireCoroutine);
    }

    public void Update()
    {
    }

    public void End()
    {
        if (_fireCoroutine != null)
            _controller.StopCoroutine(_fireCoroutine);
    }

    void Fire(int size)
    {
        var diff = _gameObject.transform.position - Game.Instance.PlayerPosition;

        int offset = (Mathf.Abs(diff.x) > Mathf.Abs(diff.y)) ?
            ((diff.x > 0f) ? 3 : 1) :
            ((diff.y > 0f) ? 0 : 2);

        for (int i = 0; i < size; i++)
        {
            float angle = ((_shotgunSpread / (size - 1)) * i) + (_shotgunSpread / 2f);
            _gameObject.Shoot(_bulletName, Vector2.zero, angle + 90f * offset);
        }
    }
    
    private IEnumerator FireShotguns()
    {
        while (true)
        {
            foreach(int size in _shotgunSize)
            {
                Fire(size);
                yield return new WaitForSeconds(_shotDelay);
            }
            yield return new WaitForSeconds(_groupDelay);
        }
    }
}
