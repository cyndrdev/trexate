public class GameConstants
{
    /* === input axes === */
    public static string LeftHorizontal = "LeftHorizontal";
    public static string LeftVertical = "LeftVertical";
    public static string RightHorizontal = "RightHorizontal";
    public static string RightVertical = "RightVertical";

    /* === global settings === */
    /* --- mechanics --- */
    public static float BulletOffScreenMargin = 0.05f;

    /* --- visuals --- */
    public static int HitFlashFrames = 3;
    public static int HitFlashFade = 0;

    /* === tags === */
    public static string PlayerController = "Player";

    /* === layers === */
    public const int PlayerLayer = 8;
    public const int PlayerBulletLayer = 9;
    public const int EnemyLayer = 10;
    public const int EnemyBulletLayer = 11;
}
