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
    private float _originRotation;
    private bool _flipX = false;

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
    public void Shoot(GameObject parent, Vector2 offset, float rotationOffset, bool flipX)
    {
        if (!_initialized)
            Debug.LogError("[Bullet]: Tried to shoot an uninitialized bullet!");

        _origin = offset;
        _originRotation = rotationOffset;
        _owner = parent;
        _flipX = flipX;

        gameObject.SetActive(true);
    }

    // perform all instantiation unique to our BulletData
    public bool Initialize(BulletData data, Transform root)
    {
        // set our bulletdata
        _data = data;

        /* === transform setup === */
        // make sure we're children of the bulletroot object
        if (root == null)
            transform.parent = Game.Instance.BulletRoot;
        else
            transform.parent = root;

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

        /* === movement === */
        BulletBehaviours.Behaviours.TryGetValue(
            _data.movementBehaviour, 
            out _simplePositionFunc);

        if (_simplePositionFunc == null)
        {
            Debug.LogError("[Bullet]: tried to initialize bullet with behaviour \""
                + _data.movementBehaviour
                + "\", but that behaviour doesn't exist.");
            return false;
        }

        /* === cleanup === */
        _initialized = true;
        return true;
    }

    /* === PRIVATE METHODS === */
    private void SetTransform(Vector2 position)
    {
        _object.transform.localPosition = _origin + position.Rotate(_originRotation);
    }

    private void UpdatePosition(float t)
    {
        t *= _data.timeScale;
        Vector2 pos = _simplePositionFunc(t) * _data.movementScale;
        if (_flipX) pos *= new Vector2(-1f, 1f);
        SetTransform(pos);
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

        /* === transform setup === */
        // set our origin to be our owner's position
        _origin += (Vector2)_owner.transform.position;
        _originRotation += _owner.transform.rotation.eulerAngles.z;

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
        SetTransform(_simplePositionFunc(0f));

        // after this point, either _smartPositionFunc or _simplePositionFunc will be set
        _startTime = Time.time;
    }

    // teleport disabled bullets somewhere hidden
    void OnDisable()
    {
        if (!_initialized)
            return;

        _object.transform.localPosition = new Vector2(-1000f, -1000f);
    }

    void Update()
    {
        if (!_initialized)
            return;

        if (_smartPositionFunc != null)
            return; // let our seperate movement monobehaviour deal with this

        float t = Time.time - _startTime;
        if (t >= _data.lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        UpdatePosition(t);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHeart enemyHeart = collision.GetComponent<EnemyHeart>();
        PlayerHeart playerHeart = collision.GetComponent<PlayerHeart>();

        // FIXME: make this more generic?
        if (_playerBullet && enemyHeart != null)
        {
            enemyHeart.DoDamage(1);
            gameObject.SetActive(false);
        }
        else if (!_playerBullet && playerHeart != null)
        {
            playerHeart.Hit();
            gameObject.SetActive(false);
        }
    }
}
