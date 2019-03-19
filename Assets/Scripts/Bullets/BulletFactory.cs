using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private Dictionary<BulletData, List<Bullet>> _bank;

    void Awake()
    {
        _bank = new Dictionary<BulletData, List<Bullet>>();
    }

    public void Shoot(GameObject parent, BulletData data, Vector2 offset, float rotationOffset)
    {
        // if this bullet hasn't been fired before, fire it!
        if (!_bank.ContainsKey(data))
            _bank.Add(data, new List<Bullet>());

        List<Bullet> vault = _bank[data];

        Bullet newBullet = null;

        // first, check if there's a disabled bullet somewhere
        foreach (Bullet bullet in vault)
        {
            if (!bullet.isActiveAndEnabled)
            {
                newBullet = bullet;
                break;
            }
        }

        // create a new one if it hasn't already been created
        if (newBullet == null)
        {
            GameObject newObject = new GameObject("Bullet");
            newBullet = newObject.AddComponent<Bullet>();
            newBullet.Initialize(data);
            vault.Add(newBullet);
        }

        newBullet.Shoot(parent, offset, rotationOffset);
    }
}

public static class BulletExtensions
{
    public static void Shoot(this GameObject parent, BulletData data)
        => Game.Instance.BulletFactory.Shoot(parent, data, new Vector2(0,0), 0f);

    public static void Shoot(this GameObject parent, BulletData data, Vector2 offset, float rotationOffset)
        => Game.Instance.BulletFactory.Shoot(parent, data, offset, rotationOffset);
}