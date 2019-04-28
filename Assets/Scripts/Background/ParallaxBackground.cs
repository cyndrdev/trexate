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
    [SerializeField]
    [Range(1f,20f)]
    private float _movementRate = 3f;

#pragma warning disable 0649
    [SerializeField]
    private Sprite[] _background;
    [SerializeField]
    private Sprite[] _layerOne;
    [SerializeField]
    private Sprite[] _layerTwo;
#pragma warning restore 0649

    private List<ParallaxLayer> _layers;
    private float _baseHeight;

    //private float _t;
    [SerializeField]
    [ReadOnly]
    private float _position;

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

            sr.flipX = (i % 2 == 0); // flip every other set

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

    void ChangeLayerSprite(GameObject baseNode, Sprite sprite)
    {
        foreach (SpriteRenderer sr in baseNode.GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sprite = sprite;
        }
    }

    public void SwapLayer(int id)
    {
        ChangeLayerSprite(_layers[0].GameObject, _background[id]);
        ChangeLayerSprite(_layers[1].GameObject, _layerOne[id]);
        ChangeLayerSprite(_layers[2].GameObject, _layerTwo[id]);
    }

    private void Start()
    {
        _layers = new List<ParallaxLayer>();
        InstantiateLayer(_background[0]);
        InstantiateLayer(_layerOne[0]);
        InstantiateLayer(_layerTwo[0]);
    }

    private void Update()
    {
        //_t += Time.deltaTime;
        //float relPos = _t * movementRate;
        float multiplier = 1f;
        if (Game.Instance != null)
            multiplier = Game.GetPersistentComponent<LevelManager>().IsJumping ? 3f : 1f;

        _position += Time.deltaTime * _movementRate * multiplier;

        while (_position > _baseHeight)
        {
            _position -= _baseHeight;

            // toggle flip on each layer
            foreach (ParallaxLayer layer in _layers)
            {
                foreach (var sr in layer.GameObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    sr.flipX = !sr.flipX;
                }
            }
        }

        foreach (ParallaxLayer layer in _layers)
        {
            var height = (layer.Size / 2f) - _position * layer.MovementScale;
            var position = layer.GameObject.transform.position;
            layer.GameObject.transform.position
                = new Vector3(position.x, height, position.z);
        }
    }
}
