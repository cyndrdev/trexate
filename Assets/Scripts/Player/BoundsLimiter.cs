using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsLimiter : MonoBehaviour
{
    private float _bounds;

    void Start()
    {
        _bounds = Game.Instance.PixelPerfectCamera.OrthoScale
            - GameConstants.BoundsMargin;
    }

    // make sure this happens after movement has been handled
    void LateUpdate()
    {
        Vector3 position = transform.position;
        Vector3 newPos = new Vector3(
            Mathf.Clamp(position.x, -_bounds, _bounds),
            Mathf.Clamp(position.y, -_bounds, _bounds),
            position.z
        );
        transform.position = newPos;
    }
}
