using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private float _shotsPerSecond;

    [SerializeField]
    private BulletData _bulletData;
#pragma warning restore 0649

    private SoundEngine _soundEngine;

    private void Start()
    {
        _soundEngine = Game.Instance.SoundEngine;
        StartCoroutine(FireRockets());
    }

    void Fire()
    {
        _soundEngine.PlaySFX("shoot3", true);
        gameObject.Shoot(_bulletData, new Vector2(0,0), Random.Range(-Mathf.PI, Mathf.PI));
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
