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

    public void Shoot(GameObject parent, BulletData data)
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
            GameObject newObject = new GameObject();
            newBullet = newObject.AddComponent<Bullet>();
            vault.Add(newBullet);
        }
    }
}

public static class BulletExtensions
{
    public static void Shoot(this GameObject parent, BulletData data)
        => Game.Instance.BulletFactory.Shoot(parent, data);
}