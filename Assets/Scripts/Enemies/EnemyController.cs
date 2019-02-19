using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float _shotsPerSecond;

    [SerializeField]
    private GameObject _rocket;

    private SoundEngine _soundEngine;

    private void Start()
    {
        StartCoroutine(FireRockets());
    }

    void Fire()
    {
        _soundEngine.PlaySFX("shoot3", true);
        Quaternion rotation = Quaternion.Euler(0f, 0f, Random.Range(-Mathf.PI, Mathf.PI));

        GameObject rocket = Instantiate(_rocket);
        rocket.transform.rotation = rotation;
        rocket.transform.position = transform.position;
    }

    private IEnumerator FireRockets()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f / _shotsPerSecond);
        }
    }
}
