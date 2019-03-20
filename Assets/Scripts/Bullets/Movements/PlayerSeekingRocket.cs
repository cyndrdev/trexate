using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PlayerSeekingRocket : MonoBehaviour
{
    private float _speed = 7f;
    private float _turningRadius = 3f;

    private float _targetAngle;
    private float _aimAngle = 0;

    // Start is called before the first frame update
    void Start()
    {
        var aimDirection =
            (Game.Instance.PlayerPosition - transform.position)
            .normalized;
        _aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
    }

    // Update is called once per frame
    void Update()
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
}
