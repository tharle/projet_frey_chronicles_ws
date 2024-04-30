using UnityEngine;

public class GameParametres
{

    public class Animation
    {
        // TODO: ça va etre utiliser au future
        public const string ENEMY_FLOAT_VELOCITY = "velocity";
        public const string ENEMY_TRIGGER_ATTACK = "attack";
        public const string ENEMY_TRIGGER_DIE = "die";
        public const string PLAYER_FLOAT_VELOCITY = "velocity";
        public const string PLAYER_TRIGGER_SHOOT = "shoot";
        public const string PLAYER_TRIGGER_INTERACT = "interact";
        public const string OBJECT_TRIGGER_OPEN = "open";
    }

    public class BundleNames
    {
        public const string PREFAB_ENEMY = "prefab_enemy";
        public const string SFX = "sfx";
    }

    public class BundlePath
    {
        public const string BUNDLE_ASSETS = "Assets/BundleAssets";
        public const string STREAMING_ASSETS = "Assets/StreamingAssets";
        public const string DATA = "/Data";
        public const string PREFAB_ENEMY = "/Prefabs/Enemies";
        public const string SFX = "/Sounds/SFX";
    }

    public class EnemyLimits 
    {
        public const int TP_MIN = 1;
        public const int TP_MAX = 100;
        public const int INICIATIVE_MIN = 1;
        public const int INICIATIVE_MAX = 10;
        public const float SPEED_MIN = 0.5f;
        public const float SPEED_MAX = 30f;
        public const float DISTANCE_MIN = 0.1f;
        public const float DISTANCE_MAX = 50f;
    }

    public class InputName
    {
        public const string AXIS_HORIZONTAL = "Horizontal";
        public const string AXIS_VERTICAL = "Vertical";
    }

    public class LayerMaskName
    {
        public const string GROUND = "Ground";
    }

    public class LayerMaskValue
    {
        public static LayerMask GROUND = LayerMask.GetMask(LayerMaskName.GROUND);
    }

    public class SceneName
    {
        public const string SCENE_GAME = "Game";
        public const string SCENE_MENU = "MainMenu";
    }
    public class TagName {
        public const string ENEMY = "Enemy";
        public const string GROUND = "Ground";
        public const string OBJECT = "Object";
        public const string PLAYER = "Player";
    }

}
