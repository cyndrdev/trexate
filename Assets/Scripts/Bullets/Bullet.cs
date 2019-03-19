using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Bullet : MonoBehaviour
{
    public BulletData _data;
    public GameObject _owner;

    public GameObject _object { get; private set; }

    private Vector2 _origin;
    private Quaternion _originRotation;

    private GameObject _graphicsHolder;
    private SpriteRenderer _renderer;
    private Collider2D _collider;

    private System.Func<float, Vector2> _simplePositionFunc;
    private MonoBehaviour _smartPositionFunc;

    private float _startTime;
    private bool _playerBullet = true;

    private bool _initialized = false;

    Collider2D CreateCollider(BulletShape shape)
    {
        switch (shape)
        {
            case BulletShape.Circle:
                var circleCollider = this.GetOrAddComponent<CircleCollider2D>();
                return circleCollider;
            case BulletShape.Square:
                var squareCollider = this.GetOrAddComponent<BoxCollider2D>();
                return squareCollider;
            case BulletShape.Rectangle:
                var rectangleCollider = this.GetOrAddComponent<BoxCollider2D>();
                return rectangleCollider;
            default:
                BoxCollider2D fallbackCollider = this.GetOrAddComponent<BoxCollider2D>();
                return fallbackCollider;
        }
    }

    /* === PUBLIC METHODS === */
    public bool CheckLifetime()
    {
        // don't check if we're already disabled
        if (!gameObject.activeInHierarchy) return false;

        bool needsDisabling = (Time.time - _startTime >= _data.lifetime);
        gameObject.SetActive(!needsDisabling);

        return needsDisabling;
    }

    public void Shoot(GameObject parent, Vector2 offset, float rotationOffset)
    {
        Debug.Log("bullet shoot!");
        transform.SetPositionAndRotation(
            offset,
            Quaternion.Euler(new Vector3(0, rotationOffset, 0)));
        _owner = parent;
        gameObject.SetActive(true);
    }

    public void Shoot(GameObject parent)
        => Shoot(parent, new Vector2(0, 0), 0f);

    // perform all instantiation unique to our BulletData
    public void Initialize(BulletData data)
    {
        Debug.Log("bullet init!");
        // set our bulletdata
        _data = data;

        /* === transform setup === */
        // make sure we're children of the bulletroot object
        transform.parent = Game.Instance.BulletRoot;

        /* === references === */
        // keep a reference to our gameobject
        _object = gameObject;

        // set our graphicsholder object
        if (transform.childCount == 0)
        {
            _graphicsHolder = new GameObject("Graphics");
            _graphicsHolder.transform.parent = transform;

            _renderer = _graphicsHolder.AddComponent<SpriteRenderer>();
        }
        else
        {
            _graphicsHolder = transform.GetChild(0).gameObject;
            _renderer = this.Find<SpriteRenderer>(_graphicsHolder);
        }

        // instantiate our colliders
        _collider = CreateCollider(_data.collisionShape);

        _renderer.sprite = _data.sprite;

        /* === scaling === */
        // scale according to data object
        transform.localScale = _data.scale * _data.collisionScale;
        _graphicsHolder.transform.localScale = new Vector2(
            1f / _data.collisionScale.x, 
            1f / _data.collisionScale.y);

        /* === cleanup === */
        _initialized = true;
    }

    /* === UNITY STATE METHODS === */
    // perform any init unique to this instantiation.
    void OnEnable()
    {
        if (!_initialized)
        {
            gameObject.SetActive(false);
            return;
        }

        Debug.Log("bullet enabled!");

        /* === transform setup === */
        // set our origin to be our owner's position
        _origin = _owner.transform.position;
        _originRotation = _owner.transform.rotation;
        transform.SetPositionAndRotation(_origin, _originRotation);

        // detect whether our owner is a player or enemy
        switch(_owner.layer)
        {
            case GameConstants.PlayerLayer:
                _playerBullet = true;
                break;
            case GameConstants.EnemyLayer:
                _playerBullet = false;
                break;
            default:
                _playerBullet = false;
                Debug.LogWarning("Bullet's parent is not on either the player or enemy layer, defaulting to enemy.");
                break;
        }

        this.gameObject.layer = 
            _playerBullet ? 
            GameConstants.PlayerBulletLayer : 
            GameConstants.EnemyBulletLayer;

        /* === movement === */
        // FIXME: get movement from a database or smth
        _simplePositionFunc = t => new Vector2(0, t);
        Debug.Log(_simplePositionFunc(1.0f));

        // after this point, either _smartPositionFunc or _simplePositionFunc will be set
        _startTime = Time.time;
    }

    void Update()
    {
        if (!_initialized)
            return;

        if (_smartPositionFunc != null)
            return; // let our seperate movement monobehaviour deal with this

        Vector2 newPosition = _simplePositionFunc(_startTime - Time.time);
        transform.localPosition = newPosition;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
