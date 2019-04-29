using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupFactory : MonoBehaviour
{
    public static Sprite _healSprite;
    public static Sprite _damageSprite;
    public static Sprite _shieldSprite;

    public static void Spawn(Vector2 position, PickupType type)
    {
        var obj = new GameObject(type.ToString() + " Pickup");
        var pickup = obj.AddComponent<Pickup>();
        pickup._type = type;

        var sr = obj.AddComponent<SpriteRenderer>();
        sr.sprite = GetSprite(type);

        var col = obj.AddComponent<CircleCollider2D>();
        col.radius = 0.21875f;

        obj.transform.parent = Game.Instance.PickupRoot;
        obj.transform.position = position;
    }

    public static Sprite GetSprite(PickupType type)
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
