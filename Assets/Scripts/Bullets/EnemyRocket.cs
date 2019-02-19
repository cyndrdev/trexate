using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class EnemyRocket : EnemyBullet
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _turningRadius;

    [SerializeField]
    private float _lifetime;

    private float _targetAngle;
    private float _aimAngle = 0;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        var aimDirection = (Game.Instance.PlayerPosition - transform.position).normalized;
        _aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
        StartCoroutine(Lifetime());
    }

    // Update is called once per frame
    new void Update()
    {
        Vector2 bulletPos = transform.position;
        Vector2 playerPos = Game.Instance.PlayerPosition;
        var targetDirection = (bulletPos - playerPos).normalized;

        _targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x)
            .GetRelativeAngle(_aimAngle);

        var proximityMod = Mathf.Clamp01((bulletPos - playerPos).magnitude / 2f);

        var movement = Vector3.right * Time.deltaTime * _speed * proximityMod.SignalToScale();

        float turn = _aimAngle - _targetAngle;
        _aimAngle += Mathf.Sign(_aimAngle - _targetAngle)
            * _turningRadius
            * Time.deltaTime
            * (2f - Mathf.Clamp01((bulletPos - playerPos).magnitude / 4f));

        transform.Translate(movement);
        transform.SetPositionAndRotation(transform.position, Quaternion.Euler(0, 0, _aimAngle.ToDegrees()));
    }

    new void Explode()
    {
        Game.Instance.SoundEngine.PlaySFX("explosion");
        Destroy(gameObject);
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(_lifetime);
        Explode();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerHeart player = collider.GetComponent<PlayerHeart>();
        if (player == null) return;

        player.Hit();
        this.Explode();
    }
}
