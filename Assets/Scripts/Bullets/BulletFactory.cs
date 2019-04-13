using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private Dictionary<BulletData, List<Bullet>> _bank;

    void Awake()
    {
        _bank = new Dictionary<BulletData, List<Bullet>>();
        StartCoroutine(DoDiagnostics());
    }

    public void Shoot(GameObject parent, BulletData data, Vector2 offset, float rotationOffset, bool flipX)
    {
        GameObject vaultObject = null;
        // if this bullet hasn't been fired before, create a vault for it
        if (!_bank.ContainsKey(data))
        {
            _bank.Add(data, new List<Bullet>());
            vaultObject = new GameObject(data.name);
            vaultObject.transform.parent = Game.Instance.BulletRoot;
        }
        else
        {
            vaultObject = Game.Instance.BulletRoot.Find(data.name).gameObject;
        }

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
            newBullet.Initialize(data, vaultObject.transform);
            newObject.transform.rotation = Quaternion.identity;
            vault.Add(newBullet);
        }

        // shoot our bullet!
        newBullet.Shoot(parent, offset, rotationOffset, flipX);
    }

    private IEnumerator DoDiagnostics()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(10f);
            int total = 0;
            int active = 0;

            foreach (var pair in _bank)
            {
                foreach (var bullet in pair.Value)
                {
                    total++;
                    if (bullet.isActiveAndEnabled) active++;
                }
            }
            Debug.Log("[BulletFactory]: " + active + " bullets active (" + total + " pooled)");
        }
    }
}

public static class BulletExtensions
{
    public static void Shoot(this GameObject parent, BulletData data)
        => Game.Instance.BulletFactory.Shoot(parent, data, new Vector2(0,0), 0f, false);

    public static void Shoot(this GameObject parent, BulletData data, Vector2 offset, float rotationOffset, bool flipX = false)
        => Game.Instance.BulletFactory.Shoot(parent, data, offset, rotationOffset, flipX);
}