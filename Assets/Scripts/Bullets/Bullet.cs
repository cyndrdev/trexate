using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Bullet : MonoBehaviour
{
    public BulletData _data;
    public GameObject _owner;

    private Vector2 _origin;
    private Quaternion _originRotation;

    private GameObject _graphicsHolder;
    private SpriteRenderer _renderer;
    private Collider2D _collider;

    private System.Func<float, Vector2> _simplePositionFunc;
    private MonoBehaviour _smartPositionFunc;

    private float _startTime;
    private bool _playerBullet = true;

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

    // use OnEnable rather than Start or Awake to make pooling easier
    void OnEnable()
    {
        /* === transform setup === */
        // make sure we're children of the bulletroot object
        transform.parent = Game.Instance.BulletRoot;

        // set our origin to be our owner's position
        _origin = _owner.transform.position;
        _originRotation = _owner.transform.rotation;
        transform.SetPositionAndRotation(_origin, _originRotation);

        // detect whether our owner is a player or enemy
        int newLayer;
        switch(_owner.layer)
        {
            case GameConstants.PlayerLayer:
                _playerBullet = true;
                newLayer = GameConstants.PlayerBulletLayer;
                break;
            case GameConstants.EnemyLayer:
                _playerBullet = false;
                newLayer = GameConstants.EnemyBulletLayer;
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

        /* === references === */
        // get our references
        _graphicsHolder = transform.GetChild(0).gameObject;

        _renderer = this.Find<SpriteRenderer>(_graphicsHolder);

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
        // FIXME: get movement from a database or smth
        // after this point, either _smartPositionFunc or _simplePositionFunc will be set
        _startTime = Time.time;
    }

    void Update()
    {
        if (_smartPositionFunc != null)
            return; // let our seperate movement monobehaviour deal with this

        transform.localPosition = _simplePositionFunc(_startTime - Time.time);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
    }
}
