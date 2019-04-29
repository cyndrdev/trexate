using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFactory : MonoBehaviour
{
    [SerializeField]
    public Sprite _healSprite;
    [SerializeField]
    public Sprite _damageSprite;
    [SerializeField]
    public Sprite _shieldSprite;

    public void SpawnRandom(Vector2 position)
        => Spawn(position, (PickupType)Random.Range(0, 3));

    public void Spawn(Vector2 position, PickupType type)
    {
        var obj = new GameObject(type.ToString() + " Pickup");
        var pickup = obj.AddComponent<Pickup>();
        pickup._type = type;

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = GetSprite(type);
        sr.sortingLayerName = GameConstants.BulletSortLayer;
        sr.sortingOrder = -1;

        var col = obj.AddComponent<CircleCollider2D>();
        col.radius = 0.21875f;
        col.isTrigger = true;

        obj.transform.parent = Game.Instance.PickupRoot;
        obj.transform.position = position;
    }

    public Sprite GetSprite(PickupType type)
    {
        switch(type)
        {
            case PickupType.Damage:
                return _damageSprite;
            case PickupType.Heal:
                return _healSprite;
            case PickupType.Shield:
                return _shieldSprite;
            default:
                return null;
        }
    }
}
