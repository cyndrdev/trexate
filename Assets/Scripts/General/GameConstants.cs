using UnityEngine;

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
    public static int SnapAngleSteps = 24;

    /* --- mechanics --- */
    public static float BulletOffScreenMargin = 0.05f;

    /* --- visuals --- */
    public static int HitFlashFrames = 2;
    public static int HitFlashFade = 2;
    public static float HitFlashPeak = 0.85f;

    /* === tags === */
    public static string PlayerController = "Player";

    /* === layers === */
    public const int PlayerLayer = 8;
    public const int PlayerBulletLayer = 9;
    public const int EnemyLayer = 10;
    public const int EnemyBulletLayer = 11;
}
