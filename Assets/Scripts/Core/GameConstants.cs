namespace Gear.Core
{
    /// <summary>
    /// Central location for all constant values used across the game.
    /// Prevents magic numbers and provides a single source of truth.
    /// </summary>
    public static class GameConstants
    {
        // Layer Names
        public const string LAYER_PLAYER = "Player";
        public const string LAYER_ENEMY = "Enemy";
        public const string LAYER_GROUND = "Ground";

        // Input Action Names
        public const string INPUT_MOVE = "Move";
        public const string INPUT_ATTACK = "Attack";
        public const string INPUT_LOOK = "Look";

        // Animation Parameter Names
        public const string ANIM_SPEED = "Speed";
        public const string ANIM_IS_ATTACKING = "IsAttacking";
        public const string ANIM_IS_DEAD = "IsDead";

        // Network Constants
        public const int MAX_PLAYERS = 4;
        public const float NETWORK_TICK_RATE = 60f;

        // Combat Constants
        public const float ATTACK_RANGE = 2.5f;
        public const float ATTACK_COOLDOWN = 1.0f;
    }
}
