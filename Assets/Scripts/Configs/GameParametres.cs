using UnityEngine;

public class GameParametres
{

    public class AnimationPlayer
    {
        public const string FLOAT_VELOCITY = "velocity";
        public const string TRIGGER_SHOOT = "shoot";
        public const string TRIGGER_INTERACT = "interact";

        public static string TRIGGER_ATTACK_ID = "attack_";
        public static string TRIGGER_TO_IDLE = "to_idle";

        public static string TRIGGER_INTERRACT = "interract";
        public static string TRIGGER_HIT_STRONG = "hit_strong";
        public static string TRIGGER_HIT_WEAK = "hit_weak";
        public static string TRIGGER_TOUCH = "touch"; 
    }

    public class AnimationDungeon
    {
        public const string TRIGGER_OPEN_DOOR = "open_door";
    }

    public class AnimationEnemy
    {
        public const string FLOAT_VELOCITY = "velocity";
        public const string TRIGGER_ATTACK = "attack";
        public const string TRIGGER_DIE = "die";

        public const string NAME_ATTACK = "attack";
        public static string TRIGGER_HIT = "hit";
    }

    public class BundleNames
    {
        public const string PREFAB_ENEMY = "prefab_enemy";
        public const string PREFAB_COMBO = "prefab_combo";
        public const string SFX = "sfx";
    }

    public class BundlePath
    {
        public const string BUNDLE_ASSETS = "Assets/BundleAssets";
        public const string STREAMING_ASSETS = "Assets/StreamingAssets";
        public const string DATA = "/Data";
        public const string PREFAB_ENEMY = "/Prefabs/Enemies";
        public const string PREFAB_COMBO = "/Prefabs/Combo";
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
        public static string WALL = "Wall";
    }

    public class LayerMaskValue
    {
        public static LayerMask GROUND = LayerMask.GetMask(LayerMaskName.GROUND);
    }

    public class SceneName
    {
        public const string SCENE_GAME = "Gym_LittleDungeon";
        public const string SCENE_MENU = "MainMenu";
    }
    public class TagName {
        public const string ENEMY = "Enemy";
        public const string GROUND = "Ground";
        public const string OBJECT = "Object";
        public const string PLAYER = "Player";
    }

}
