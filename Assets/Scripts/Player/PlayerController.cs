using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed = 10f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _rigidness = 0.77f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _aimSpeed = 0.9f;

    private Vector2 _velocity;
    private Vector2 _aimDirection;
    private float _aimAngle;

    private static InputManager _inputManager;
    private bool _isFiring;

    void Start()
    {
        _inputManager = Game.Instance.InputManager;
        _inputManager.Primary.AddListener(Fire);
    }

    private float relativeAngle(float angle)
    {
        float _lowerBound = _aimAngle - Mathf.PI;
        float _upperBound = _aimAngle + Mathf.PI;

        while (angle > _upperBound) angle -= Mathf.PI * 2f;
        while (angle < _lowerBound) angle += Mathf.PI * 2f;

        return angle;
    }

    void Update()
    {
        Vector3 targetVelocity = new Vector3(_inputManager.LeftStick.x, _inputManager.LeftStick.y) * _maxSpeed;
        float lerpmod = Mathf.Clamp01(_rigidness * _rigidness);
        _velocity = Vector3.Lerp(_velocity, targetVelocity, lerpmod);

        float targetAngle = relativeAngle(Mathf.Atan2(_inputManager.RightStick.y, _inputManager.RightStick.x));
        _aimAngle = Mathf.Lerp(_aimAngle, targetAngle, _aimSpeed);

        while (_aimAngle < Mathf.PI) _aimAngle += Mathf.PI * 2f;
        while (_aimAngle > Mathf.PI) _aimAngle -= Mathf.PI * 2f;

        _aimDirection = new Vector2(Mathf.Cos(_aimAngle), Mathf.Sin(_aimAngle));

        Debug.DrawLine(
            transform.position, 
            transform.position + new Vector3(_aimDirection.x, _aimDirection.y) * 1000
        );
    }

    private void FixedUpdate()
    {
        transform.Translate(_velocity * Time.fixedDeltaTime);
    }

    void Fire (bool down)
    {
        _isFiring = down;
    }
}
