using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    public bool Inverted;

    [SerializeField]
    Vector3 _spawnOffset;

    [SerializeField]
    float _speed = 20f;

    [SerializeField]
    float _wobbleAmount = 4f;
    [SerializeField]
    float _wobbleFrequency = 1f;

    float _startTime;

    private void Start()
    {
        _startTime = Time.time - (Mathf.Sqrt(2f) / 2f);
        transform.Translate(_spawnOffset * (Inverted ? 1f : -1f));
    }

    void Update()
    {
        base.Update();
        var verticalMovement = Vector3.right * _speed * Time.deltaTime;
        var horizontalMovement = Vector3.up * Mathf.Sin((Time.time - _startTime) * _wobbleFrequency) * _wobbleAmount * Time.deltaTime;
        transform.Translate(verticalMovement + horizontalMovement * (Inverted ? 1f : -1f));
    }
}
