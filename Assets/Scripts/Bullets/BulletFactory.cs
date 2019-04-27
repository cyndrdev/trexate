using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    private Dictionary<BulletData, List<Bullet>> _bank;

    public GameObject _preloadRoot;
    private static Dictionary<string, int> _preloadValues =
        new Dictionary<string, int>
    {
        { BulletTypes.PlayerBullets, 100 },
        { BulletTypes.EnemyRocket, 20 },
        { BulletTypes.ExplosionDebris, 100 },
        { BulletTypes.Simple, 300 }
    };

    void Awake()
    {
        _bank = new Dictionary<BulletData, List<Bullet>>();

#if DEBUG
        StartCoroutine(DoDiagnostics());
#endif
    }

    void Start()
        => StartCoroutine(Preload());

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

    public void ClearBullets()
    {
        foreach (var pair in _bank)
        {
            foreach (var bullet in pair.Value)
            {
                bullet.gameObject.SetActive(false);
            }
        }
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

    private IEnumerator Preload()
    {
        // mute sfx first
        var soundEngine = Game.GetPersistentComponent<SoundEngine>();
        bool oldState = soundEngine.MuteSFX;
        soundEngine.MuteSFX = true;

        // preloads a set list of bullets to avoid allocations during gameplay
        // shoot a bunch o bullets
        foreach (var pair in _preloadValues)
        {
            for (int i = 0; i < pair.Value; i++)
                _preloadRoot.Shoot(pair.Key);
        }

        // wait 2 frames for full init to happen
        for (int i = 0; i < 2; i++)
            yield return new WaitForEndOfFrame();

        ClearBullets();

        soundEngine.MuteSFX = oldState;
    }
}

public static class BulletExtensions
{
    private static BulletFactory _factory = Game.GetPersistentComponent<BulletFactory>();
    private static BulletData ToBulletData(this string name)
    {
        BulletData data = Resources.Load<BulletData>(GameConstants.BulletBehaviourPath + name);

        if (data == null)
        {
            throw new System.Exception("[BulletExtensions]: tried to load BulletData with name '" + name + "', none found.");
        }

        return data;
    }

    public static void Shoot(this GameObject parent, BulletData data)
        => _factory.Shoot(
            parent, 
            data, 
            new Vector2(0,0), 
            0f, 
            false
        );

    public static void Shoot(this GameObject parent, BulletData data, Vector2 offset, float rotationOffset, bool flipX = false)
        => _factory.Shoot(
            parent, 
            data, 
            offset, 
            rotationOffset, 
            flipX
        );

    public static void Shoot(this GameObject parent, string dataName)
        => _factory.Shoot(
            parent,
            dataName.ToBulletData(), 
            new Vector2(0, 0), 
            0f, 
            false
        );

    public static void Shoot(this GameObject parent, string dataName, Vector2 offset, float rotationOffset, bool flipX = false)
        => _factory.Shoot(
            parent,
            dataName.ToBulletData(),
            offset,
            rotationOffset,
            flipX
        );
}