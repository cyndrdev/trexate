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

        float spriteHeight = sprite.bounds.size.y;

        if (_layers.Count == 0)
        {
            // this is our base layer, set our base height
            _baseHeight = spriteHeight;
        }

        for (int i = 0; i <3; i++)
        {
            var so = new GameObject("Instance " + i);
            so.transform.parent = go.transform;
            var sr = so.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingLayerName = GameConstants.BackgroundSortLayer;
            sr.sortingOrder = _layers.Count;

            so.transform.localPosition = new Vector3(0f, (i - 1) * spriteHeight, 0f);
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
        {
            _t -= relPos / movementRate;
            relPos -= _baseHeight;
        }

        foreach (ParallaxLayer layer in _layers)
        {
            var height = (layer.Size / 2f) - relPos * layer.MovementScale;
            var position = layer.GameObject.transform.position;
            layer.GameObject.transform.position
                = new Vector3(position.x, height, position.z);
        }
    }
}
