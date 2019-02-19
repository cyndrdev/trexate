using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class Bullet : MonoBehaviour
{
    public BulletData _data;

    private GameObject _graphicsHolder;
    private SpriteRenderer _renderer;
    private Collider2D _collider;

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

    void Awake()
    {
        _graphicsHolder = transform.GetChild(0).gameObject;

        _renderer = this.Find<SpriteRenderer>(_graphicsHolder);
        _collider = CreateCollider(_data.collisionShape);

        _renderer.sprite = _data.sprite;

        transform.parent = Game.Instance.BulletRoot;
        transform.localScale = _data.scale * _data.collisionScale;
        _graphicsHolder.transform.localScale = new Vector2(
            1f / _data.collisionScale.x, 
            1f / _data.collisionScale.y);
    }

    void Start()
    {
        
    }

    void Update()
    {
    }
}
