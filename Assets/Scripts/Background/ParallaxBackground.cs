using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ParallaxLayer
{
    public GameObject GameObject;
    public float MovementScale;

    public ParallaxLayer(GameObject gameObject, float movementScale)
    {
        GameObject = gameObject;
        MovementScale = movementScale;
    }
}

public class ParallaxBackground : MonoBehaviour
{
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
        foreach (ParallaxLayer layer in _layers)
        {
            var position = layer.GameObject.transform.position;
            layer.GameObject.transform.position = new Vector3
            (
                position.x,
                -_t * layer.MovementScale,
                position.z
            );
        }
    }
}
