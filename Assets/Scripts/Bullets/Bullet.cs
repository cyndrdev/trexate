using System;
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

    private SoundEngine _soundEngine;
    private Material _material;

    private float _startTime;
    private bool _playerBullet = true;

    private bool _initialized = false;

    Collider2D CreateCollider(BulletShape shape)
    {
        Collider2D newCollider = null;
        switch (shape)
        {
            case BulletShape.Circle:
                CircleCollider2D circle = this.GetOrAddComponent<CircleCollider2D>();
                circle.radius = 0.1f;
                newCollider = circle;
                break;
            case BulletShape.Square:
                newCollider = this.GetOrAddComponent<BoxCollider2D>();
                break;
            case BulletShape.Rectangle:
                newCollider = this.GetOrAddComponent<BoxCollider2D>();
                break;
            default:
                newCollider = this.GetOrAddComponent<BoxCollider2D>();
                break;
        }

        newCollider.isTrigger = true;
        return newCollider;
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

        _soundEngine = Game.Instance.SoundEngine;


        // set our graphicsholder object
        if (transform.childCount == 0)
        {
            _graphicsHolder = new GameObject("Graphics");
            _graphicsHolder.transform.parent = transform;

            _renderer = _graphicsHolder.AddComponent<SpriteRenderer>();
            _renderer.sprite = _data.sprite;
            _renderer.sortingLayerName = GameConstants.BulletSortLayer;

            //_material = new Material(Shader.Find(GameConstants.BulletShader));

            //_material.mainTexture = _renderer.sprite.texture;
            //_renderer.material = _material;

            //gameObject.AddComponent<PixelPerfectEntity>();
            //_graphicsHolder.AddComponent<SpriteUVToShader>();
        }
        else
        {
            _graphicsHolder = transform.GetChild(0).gameObject;
            _renderer = this.Find<SpriteRenderer>(_graphicsHolder);
        }

        // instantiate our colliders
        _collider = CreateCollider(_data.collisionShape);

        /* === scaling === */
        // scale according to data object
        transform.localScale = _data.scale * _data.collisionScale;
        _graphicsHolder.transform.localScale = new Vector2(
            1f / _data.collisionScale.x, 
            1f / _data.collisionScale.y);

        /* === movement === */
        if (_data.useSimpleMovement)
        {
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
        }
        else
        {
            // NOTE: this is potentially a really bad idea!
            // we're adding a component via reflection.
            Type objType = Type.GetType(_data.movementScript);

            if (objType != null)
            {
                _object.AddComponent(objType);
            }
            else
            {
                Debug.LogError("[Bullet]: tried to initialize bullet with movement script \""
                    + _data.movementScript
                    + "\", but that script doesn't exist.");
            }
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
        // _originRotation += _owner.transform.rotation.eulerAngles.z;

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
        if (_data.useSimpleMovement)
            SetTransform(_simplePositionFunc(0f));
        else
            SetTransform(new Vector2(0f, 0f));

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

        float t = Time.time - _startTime;
        if (t >= _data.lifetime)
        {
            gameObject.SetActive(false);
            return;
        }

        // only handle movement if we're using the "simple" version
        if (_data.useSimpleMovement)
            UpdatePosition(t);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyHeart enemyHeart = collision.GetComponent<EnemyHeart>();
        PlayerHeart playerHeart = collision.GetComponent<PlayerHeart>();

        bool playerHit = (!_playerBullet && playerHeart != null);
        bool enemyHit = (_playerBullet && enemyHeart != null);

        if (playerHit) playerHeart.Hit();
        if (enemyHit) enemyHeart.DoDamage(1);

        if (playerHit || enemyHit)
        {
            gameObject.SetActive(false);
            if (_data.explodeOnInpact) _soundEngine.PlaySFX("explosion");
        }
    }
}
