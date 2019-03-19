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
    [ConditionalHide("useSimpleMovement", false)]
    public string movementBehaviour;
    [ConditionalHide("useSimpleMovement", false)]
    public Vector2 movementScale;
    [ConditionalHide("useSimpleMovement", false)]
    public float timeScale;
    [ConditionalHide("useSimpleMovement", false, true)]
    public string movementScript;
    public bool explodeOnInpact;
}
