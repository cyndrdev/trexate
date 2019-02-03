using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Extensions;

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

    [SerializeField]
    [Range(0.5f, 100f)]
    private float _shotsPerSecond = 10f;

    [SerializeField]
    private GameObject _playerBullet;

    private Vector2 _velocity;
    private Vector2 _aimDirection;
    private float _aimAngle;

    private static InputManager _inputManager;
    private static SoundEngine _soundEngine;
    private bool _isFiring;

    private IEnumerator _fireContinuously;

    void Start()
    {
        _inputManager = Game.Instance.InputManager;
        _soundEngine = Game.Instance.SoundEngine;

        _inputManager.Primary.AddListener(ChangeFireState);
        _fireContinuously = FireContinuously();

        if (_playerBullet == null)
        {
            throw new System.Exception("[PlayerController]: no bullet prefab set for player!");
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

    void ChangeFireState (bool down)
    {
        _isFiring = down;
        if (_isFiring) StartCoroutine(_fireContinuously);
        else StopCoroutine(_fireContinuously);
    }

    void Fire()
    {
        //Debug.Log("pew!");
        //_soundEngine.PlayRandomSFX("shoot", 1, 4, true);
        _soundEngine.PlaySFX("shoot1", true);
        Quaternion _rotation = Quaternion.Euler(0, 0, _aimAngle.ToDegrees());

        GameObject leftBullet = Instantiate(_playerBullet);
        GameObject rightBullet = Instantiate(_playerBullet);

        leftBullet.transform.position = transform.position;
        leftBullet.transform.rotation = _rotation;

        rightBullet.transform.position = transform.position;
        rightBullet.transform.rotation = _rotation;
        rightBullet.GetComponent<PlayerBullet>().Inverted = true;
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
