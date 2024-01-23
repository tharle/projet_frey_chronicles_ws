using UnityEngine;

public class GameParametres
{

    public class Animation
    {
        public const string ENEMY_FLOAT_VELOCITY = "velocity";
        public const string ENEMY_TRIGGER_ATTACK = "attack";
        public const string ENEMY_TRIGGER_DIE = "die";
        public const string PLAYER_FLOAT_VELOCITY = "velocity";
        public const string PLAYER_TRIGGER_SHOOT = "shoot";
        public const string PLAYER_TRIGGER_INTERACT = "interact";
        public const string OBJECT_TRIGGER_OPEN = "open";
    }
    public class InputName
    {
        public const string AXIS_HORIZONTAL = "Horizontal";
        public const string AXIS_VERTICAL = "Vertical";
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
        public const string TABLE = "Table";
    }
    public class Values
    {
        public const int TIME_TO_SUIVIVE_IN_SECONDS = 60;
        public const float ENEMY_HUNGER_MIN = 5F;
        public const float ENEMY_HUNGER_MAX = 10f;
        public const float ENEMY_COOLDOWN_BITE = 1f;
    }

}
