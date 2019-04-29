using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Extensions;

public class PickupFactory : MonoBehaviour
{
    [SerializeField]
    public Sprite _healSprite;
    [SerializeField]
    public Sprite _damageSprite;
    [SerializeField]
    public Sprite _shieldSprite;

    public void SpawnRandom(Vector2 position, byte pool)
    {
        if (pool == 0 || pool >= 8) return;

        while (true)
        {
            int i = Random.Range(0, 3);
            if ((pool & (byte)2.Pow(i)) == 0)
            {
                Spawn(position, (PickupType)i);
                return;
            }
        }
    }

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
        col.radius = GameConstants.PickupArea;
        col.isTrigger = true;

        Vector2 offset = Vector2.up * GameConstants.PickupSpawnRadius * Random.Range(0f, 1f);
        offset = offset.Rotate(Random.Range(0f, 360f));

        obj.transform.parent = Game.Instance.PickupRoot;
        obj.transform.position = position + offset;
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

    public void CleanField()
    {
        foreach (Transform child in Game.Instance.PickupRoot)
            GameObject.Destroy(child.gameObject);
    }
}
