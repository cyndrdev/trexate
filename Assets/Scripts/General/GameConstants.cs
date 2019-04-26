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

    /* --- Sprite UV Passthrough --- */
    public static string UVPassthroughUV = "_SpriteUV";
    public static string UVPassthroughPivot = "_SpritePivot";
    public static string UVPassthroughUVCenter = "_UVCenter";
    public static string UVPassthroughTextureSize = "_Res";
    public static string UVPassthroughPixelSize = "_PixelSize";

    /* --- mechanics --- */
    public static float BoundsMargin = 0.25f;
    public static float SpawnMarginY = 2f;

    /* --- time travel --- */
    public static int TimeTravelStart = 7192;
    public static int TimeTravelEnd = -137500000;
    public static float TimeTravelExponent = 1.7f;
    public static float TimeTravelDuration = 350f;
    public static float TimeJumpDuration = 1.5f;

    /* --- points --- */
    public static int MinScoreIncrease = 1;
    public static int GruntHitScore = 10;
    public static int GruntKillScore = 500;

    /* --- input --- */
    public static float EmergencyExitDuration = 1f;

    /* === visuals === */
    /* --- gameplay --- */
    public static int HitFlashFrames = 2;
    public static int HitFlashFade = 2;
    public static float HitFlashPeak = 0.85f;
    public static string PixelPerfectShader = "Sprites/PixelPerfectEntity";
    public static string EnemyShader = "Sprites/Entity";
    public static float EnemyInvulnFlashRate = 3f;
    public static Color EnemyInvulnFlashColor = new Color(0f, 0.4f, 1f);

    /* --- ui --- */
    public static float CounterRefreshRate = 0.025f;
    public static float PauseDisplayFlashRate = 0.35f;
    public static float FramerateDisplayDelay = 1.3f;

    public static float AreaSwitchFadeDuration = 1.0f;

    public static float NewAreaTextFadeInDuration = 0.2f;
    public static float NewAreaTextFadeOutDuration = 0.5f;
    public static float NewAreaTextHoldDuration = 0.7f;

    public static float CreditsFadeInSpeed = 1.5f;
    public static float CreditsTopMargin = 200.0f;
    public static float CreditsScrollSpeed = 100.0f;
    public static float CreditsEndTextFadeSpeed = 3.0f;

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

    /* === resource paths === */
    public const string BulletBehaviourPath = "BulletTypes/";
    public const string EnemyBehaviourPath = "EnemyBehaviour/";
}
