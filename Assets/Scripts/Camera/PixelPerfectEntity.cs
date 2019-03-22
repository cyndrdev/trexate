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

        position.x = (
            Mathf.Round(transform.position.x * GameConstants.TileSize) / GameConstants.TileSize
            - transform.position.x
        );

        position.y = (
            Mathf.Round(transform.position.y * GameConstants.TileSize) 
            / GameConstants.TileSize
            - transform.position.y
        );

        _graphics.localPosition = position;
    }

    void snapAngle()
    {
        float stepValue = 360f / GameConstants.SnapAngleSteps;

        float parentRotation = transform.rotation.eulerAngles.z;

        float targetAngle = Mathf.Round(parentRotation / stepValue) * stepValue;

        Debug.Log(targetAngle);

        float angleDiff = targetAngle - parentRotation;

        _graphics.localRotation = angleDiff.ToRotation();
    }

    void Start()
    {
        _renderer = GetComponentInChildren<SpriteRenderer>();

        if (_renderer == null)
            throw new System.Exception();

        _graphics = _renderer.transform;
    }

    void LateUpdate()
    {
        if (GameConstants.SnapAngle) snapAngle();
        if (GameConstants.SnapPosition) snapPosition();
    }
}
