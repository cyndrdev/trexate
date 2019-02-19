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

    new private void Start()
    {
        base.Start();
        _startTime = Time.time;
        transform.Translate(_spawnOffset * (Inverted ? 1f : -1f));
    }

    new void Update()
    {
        base.Update();
        var verticalMovement = Vector3.right * _speed * Time.deltaTime;
        var horizontalMovement = Vector3.up * Mathf.Sin((Time.time - _startTime) * _wobbleFrequency) * _wobbleAmount * Time.deltaTime;
        transform.Translate(verticalMovement + horizontalMovement * (Inverted ? 1f : -1f));
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        EnemyHeart enemy = collider.GetComponent<EnemyHeart>();
        if (enemy == null) return;

        enemy.DoDamage(1);
        Destroy(this.gameObject);
    }
}
