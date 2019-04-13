using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParallaxLayer
{
    public GameObject GameObject;
    public float Size;
    public float MovementScale;

    public ParallaxLayer(GameObject gameObject, float size, float movementScale)
    {
        GameObject = gameObject;
        Size = size;
        MovementScale = movementScale;
    }
}

public class ParallaxBackground : MonoBehaviour
{
    public float movementRate = 16f;
    public Sprite _background;
    public Sprite _layerOne;
    public Sprite _layerTwo;

    private List<ParallaxLayer> _layers;
    private float _baseHeight;

    private float _t;

    void InstantiateLayer(Sprite sprite)
    {
        var go = new GameObject("Layer " + _layers.Count.ToString());
        go.transform.parent = transform;
        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;

        float spriteHeight = sprite.bounds.size.y;

        if (_layers.Count == 0)
        {
            // this is our base layer, set our base height
            _baseHeight = spriteHeight;
        }

        _layers.Add(
            new ParallaxLayer(
                go, 
                spriteHeight,
                spriteHeight / _baseHeight
            )
        );
    }

    private void Start()
    {
        _layers = new List<ParallaxLayer>();
        InstantiateLayer(_background);
        InstantiateLayer(_layerOne);
        InstantiateLayer(_layerTwo);
    }

    private void Update()
    {
        _t += Time.deltaTime;
        float relPos = _t * movementRate;

        while (relPos > _baseHeight)
            relPos -= _baseHeight;

        foreach (ParallaxLayer layer in _layers)
        {
            var height = (layer.Size / 2f) - relPos * layer.MovementScale;
            var position = layer.GameObject.transform.position;
            layer.GameObject.transform.position
                = new Vector3(position.x, height, position.z);
        }
    }
}
