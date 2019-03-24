using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField]
    private bool _fill;
#pragma warning restore 0649

    private float _orthoScale;
    private Camera _camera;

    public int SpriteScale { get; private set; }

    void UpdateOrtho()
    {
        _camera = GetComponent<Camera>();

        if (_camera == null)
            throw new System.Exception();

        float pixelSizeX = Screen.width / GameConstants.ViewportRes.x;
        float pixelSizeY = Screen.height / GameConstants.ViewportRes.y;

        SpriteScale = _fill ?
            (int)Mathf.Min(pixelSizeX, pixelSizeY) :
            (int)Mathf.Max(pixelSizeX, pixelSizeY);

        _orthoScale = (float)Screen.height / SpriteScale / GameConstants.TileSize / 2f;

        _camera.orthographicSize = _orthoScale;
    }

    void Awake() => UpdateOrtho();
}
