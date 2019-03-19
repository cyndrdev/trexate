using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum BulletShape
{
    Circle,
    Square,
    Rectangle
}

[CreateAssetMenu(fileName = "Data", menuName = "Bullet", order = 1)]
public class BulletData : ScriptableObject
{
    [Header("General")]
    public float lifetime;

    [Header("Visuals")]
    public Sprite sprite;
    public Vector2 scale;

    [Header("Collision")]
    public BulletShape collisionShape;
    public Vector2 collisionScale;

    [Header("Behaviour")]
    public bool useSimpleMovement;
    [ConditionalHide("useSimpleMovement", true)]
    public string movementBehaviour;
    [ConditionalHide("useSimpleMovement", true)]
    public Vector2 movementScale;
    [ConditionalHide("useSimpleMovement", true)]
    public float timeScale;
    public bool explodeOnInpact;
}
