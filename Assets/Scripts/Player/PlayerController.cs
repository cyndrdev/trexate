using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float _maxSpeed = 5f;
    [SerializeField]
    private float _sloppiness = 0.1f;

    private Vector2 _velocity;

    private static InputManager _inputManager;
    private bool _isFiring;

    void Start()
    {
        _inputManager = Game.Instance.InputManager;
        _inputManager.Primary.AddListener(Fire);
    }

    void Update()
    {
        Vector3 targetVelocity = new Vector3(_inputManager.LeftStick.x, _inputManager.LeftStick.y) * _maxSpeed;
        float lerpmod = Mathf.Clamp01(1f / (1f + _sloppiness) * Time.deltaTime * 6f);
        _velocity = Vector3.Lerp(_velocity, targetVelocity, lerpmod);
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
