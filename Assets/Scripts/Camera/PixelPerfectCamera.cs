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
    public float OrthoScale { get => _orthoScale; }

    void UpdateOrtho()
    {
        _camera = GetComponent<Camera>();

        if (_camera == null)
            throw new System.Exception();

        float pixelSizeX = (float)Screen.width / (float)GameConstants.ViewportRes.x;
        float pixelSizeY = (float)Screen.height / (float)GameConstants.ViewportRes.y;
        Debug.Log("X: " + pixelSizeX + "\nY: " + pixelSizeY);

        SpriteScale = _fill ?
            (int)Mathf.Ceil(Mathf.Min(pixelSizeX, pixelSizeY)) :
            (int)Mathf.Ceil(Mathf.Max(pixelSizeX, pixelSizeY));

        Debug.Log("[PixelPerfectCamera]: initialised with pixel size of " + SpriteScale);

        _orthoScale = (float)Screen.height / (SpriteScale * GameConstants.TileSize);

        _camera.orthographicSize = _orthoScale;
    }

    void Awake() => UpdateOrtho();
}
