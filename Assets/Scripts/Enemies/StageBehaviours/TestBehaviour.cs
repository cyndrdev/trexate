using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBehaviour : IEnemyState
{
    private float _shotsPerSecond = 2.0f;
    private IEnumerator _behaviourCoroutine;
    private string _bulletName = "enemyrocket";
    private GameObject _gameObject;
    private MonoBehaviour _script;

    public void Start(GameObject gameObject, MonoBehaviour script)
    {
        _script = script;
        _gameObject = gameObject;

        _behaviourCoroutine = FireRockets();
        _script.StartCoroutine(_behaviourCoroutine);
    }

    public void End()
    {
        _script.StopCoroutine(_behaviourCoroutine);
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
