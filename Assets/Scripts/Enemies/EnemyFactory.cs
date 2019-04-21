using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    private Dictionary<EnemyData, List<EnemyController>> _census;

    private void Awake()
    {
        _census = new Dictionary<EnemyData, List<EnemyController>>();
    }

    public void Spawn(EnemyData data, float xPosition)
    {

    }
}

public static class EnemyExtensions
{
    private static EnemyData ToEnemyData(this string name)
    {
        EnemyData data = Resources.Load<EnemyData>(name);

        if (data == null)
            throw new System.Exception("[EnemyExtensions]: tried to load EnemyData with name '" + name + "', none found.");

        return data;
    }

    public static void Spawn(this GameObject gameObject, EnemyData data, float xPosition)
        => Game.GetPersistentComponent<EnemyFactory>().Spawn(data, xPosition);

    public static void Spawn(this GameObject gameObject, string dataName, float xPosition)
        => Game.GetPersistentComponent<EnemyFactory>().Spawn(dataName.ToEnemyData(), xPosition);
}
