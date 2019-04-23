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
        GameObject genus = null;

        // make sure we have the correct parent
        if (!_census.ContainsKey(data))
        {
            _census.Add(data, new List<EnemyController>());
            genus = new GameObject(data.name);
            genus.transform.parent = Game.Instance.EnemyRoot;
        }
        else
        {
            genus = Game.Instance.EnemyRoot.Find(data.name).gameObject;
        }

        List<EnemyController> clones = _census[data];

        // create the enemy object
        GameObject newObject = new GameObject("Enemy");

        float scale = Game.Instance.PixelPerfectCamera.OrthoScale;
        newObject.transform.position
            = new Vector2(scale * xPosition, scale);

        EnemyController newEnemy = newObject.AddComponent<EnemyController>();
        
        // give it its data and initialise it
        newEnemy.Initialise(data);

        // add to the correct parent
        newObject.transform.parent = genus.transform;
        clones.Add(newEnemy);
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
