using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _shotsPerSecond;

    [SerializeField]
    private BulletData _bulletData;

    [SerializeField]
    private GameObject _rocket;

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
        /*
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(-Mathf.PI, Mathf.PI));

        GameObject rocket = Instantiate(_rocket);
        rocket.transform.rotation = rotation;
        rocket.transform.position = transform.position;
        */
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
