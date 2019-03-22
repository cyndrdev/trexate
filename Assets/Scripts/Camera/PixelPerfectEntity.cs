using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PixelPerfectEntity : MonoBehaviour
{
    private SpriteRenderer _renderer;
    private Transform _graphics;

    void snapPosition()
    {
        Vector3 position = _graphics.localPosition;

        int unitSnap = GameConstants.TileSize; 

        position.x = ((Mathf.Round(transform.position.x * unitSnap) / unitSnap) - transform.position.x);
        position.y = ((Mathf.Round(transform.position.y * unitSnap) / unitSnap) - transform.position.y);

        _graphics.localPosition = position;
    }

    void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();

        if (_renderer == null)
            throw new System.Exception();

        _graphics = _renderer.transform;

        if (GameConstants.SnapAngle) _graphics.gameObject.AddComponent<SpriteUVToShader>();
    }

    void LateUpdate()
    {
        if (GameConstants.SnapPosition) snapPosition();
    }
}
