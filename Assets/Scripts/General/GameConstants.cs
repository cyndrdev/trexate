﻿using UnityEngine;

public class GameConstants
{
    /* === input axes === */
    public static string LeftHorizontal = "LeftHorizontal";
    public static string LeftVertical = "LeftVertical";
    public static string RightHorizontal = "RightHorizontal";
    public static string RightVertical = "RightVertical";

    /* === global settings === */
    /* --- pixel perfect --- */
    public static Vector2Int ViewportRes = new Vector2Int(360, 360);
    public static int TileSize = 16;
    public static bool SnapPosition = true;
    public static bool SnapAngle = true;

    /* --- Sprite UV Passthrough --- */
    public static string UVPassthroughUV = "_SpriteUV";
    public static string UVPassthroughPivot = "_SpritePivot";
    public static string UVPassthroughUVCenter = "_UVCenter";
    public static string UVPassthroughTextureSize = "_Res";
    public static string UVPassthroughPixelSize = "_PixelSize";

    /* --- mechanics --- */
    public static float BulletOffScreenMargin = 0.05f;

    /* --- points --- */
    public static int MinScoreIncrease = 1;
    public static int GruntHitScore = 10;
    public static int GruntKillScore = 500;

    /* === visuals === */
    public static int HitFlashFrames = 2;
    public static int HitFlashFade = 2;
    public static float HitFlashPeak = 0.85f;
    public static string PixelPerfectShader = "Sprites/PixelPerfectEntity";
    public static float CounterRefreshRate = 0.025f;

    /* === tags === */
    public static string PlayerController = "Player";

    /* === physics layers === */
    public const int PlayerLayer = 8;
    public const int PlayerBulletLayer = 9;
    public const int EnemyLayer = 10;
    public const int EnemyBulletLayer = 11;

    /* === sprite sort layers === */

    public const string EntitySortLayer = "Entities";
    public const string BulletSortLayer = "Bullets";
    public const string BackgroundSortLayer = "Background";
}
