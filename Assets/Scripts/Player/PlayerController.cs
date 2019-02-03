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
        float lerpmod = Mathf.Clamp01(_rigidness * _rigidness);
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
