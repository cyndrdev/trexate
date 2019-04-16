using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

public class PlayerController : MonoBehaviour
{
    /* === movement & aim tweaks === */
    [SerializeField]
    private float _maxSpeed = 10f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _rigidness = 0.77f;

    [SerializeField]
    [Range(0f, 1f)]
    private float _aimSpeed = 0.9f;

    [SerializeField]
    [Range(0.5f, 100f)]
    private float _shotsPerSecond = 10f;

#pragma warning disable 0649
    [SerializeField]
    private BulletData _bulletData;
#pragma warning restore 0649

    private Vector2 _velocity;
    private Vector2 _aimDirection;
    private float _aimAngle;

    private static InputManager _inputManager;
    private static SoundEngine _soundEngine;
    private Transform _graphics;
    private bool _isFiring;

    private IEnumerator _fireContinuously;

    void Start()
    {
        _inputManager = Game.Instance.InputManager;
        _soundEngine = Game.Instance.SoundEngine;
        _graphics = transform.GetChild(0);

        _inputManager.Primary.AddListener(ChangeFireState);
        _fireContinuously = FireContinuously();

        if (_bulletData == null)
        {
            throw new System.Exception("[PlayerController]: no bullet data set for player!");
        }
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
        float frameTime = Time.deltaTime * 60f;

        Vector3 targetVelocity = new Vector3(_inputManager.LeftStick.x, _inputManager.LeftStick.y) * _maxSpeed;
        float lerpmod = Mathf.Clamp01(_rigidness * _rigidness * frameTime);
        _velocity = Vector3.Lerp(_velocity, targetVelocity, lerpmod);

        float targetAngle = Mathf.Atan2(_inputManager.RightStick.y, _inputManager.RightStick.x).GetRelativeAngle(_aimAngle);
        _aimAngle = Mathf.Lerp(_aimAngle, targetAngle, _aimSpeed * frameTime);

        while (_aimAngle < Mathf.PI) _aimAngle += Mathf.PI * 2f;
        while (_aimAngle > Mathf.PI) _aimAngle -= Mathf.PI * 2f;

        _aimDirection = new Vector2(Mathf.Cos(_aimAngle), Mathf.Sin(_aimAngle));

        Debug.DrawLine(
            transform.position, 
            transform.position + new Vector3(_aimDirection.x, _aimDirection.y) * 1000
        );

        transform.localRotation = (_aimAngle.ToDegrees() - 90f).ToRotation();
    }

    private void FixedUpdate()
    {
        transform.localPosition += new Vector3(_velocity.x, _velocity.y) * Time.fixedDeltaTime;
    }

    void ChangeFireState (bool down)
    {
        _isFiring = down;
        if (_isFiring) StartCoroutine(_fireContinuously);
        else StopCoroutine(_fireContinuously);
    }

    void Fire()
    {
        float _shootAngle = _aimAngle.ToDegrees() - 90f;
        gameObject.Shoot(_bulletData, new Vector2(0, 0), _shootAngle, true);
        gameObject.Shoot(_bulletData, new Vector2(0, 0), _shootAngle, false);
    }

    private IEnumerator FireContinuously()
    {
        while(true)
        {
            Fire();
            yield return new WaitForSeconds(1f / _shotsPerSecond);
        }
    }
}
