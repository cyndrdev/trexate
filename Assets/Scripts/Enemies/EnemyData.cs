using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* ~~~ sinning starts here ~~~ */
// no mortals may pass

[System.Serializable]
public struct StageContext
{
    // i guess instantiating MonoBehaviours via strings is
    // just, like, a thing we do now?
    public string StageScript;
    public int HealthTrigger;
}

[CreateAssetMenu(fileName = "Enemy", menuName = "Enemies/Data", order = 1)]
public class EnemyData : ScriptableObject
{
    [Header("Health")]
    public int MaxHealth;

    [Header("Visuals")]
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;
    public Vector2 scale = new Vector2(1f, 1f);

    [Header("Sound FX")]
    public string hitSound;
    public string deathSound;

    [Header("Points")]
    public int hitScore;
    public int killScore;

    [Header("Collision")]
    public Vector2 collisionScale = new Vector2(1f, 1f);

    [Header("Behaviours")]
    public List<StageContext> Stages;
}