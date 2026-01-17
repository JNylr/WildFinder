using Gear.StateMachine;
using Gear.Core;

namespace Gear.Player.States
{
    /// <summary>
    /// Player-specific state machine implementation.
    /// Manages player behavior states (Idle, Moving, Attacking).
    /// </summary>
    public class PlayerStateMachine : BaseStateMachine
    {
        public IdleState idleState;
        public MovingState movingState;
        public AttackingState attackingState;

        private PlayerMovement movement;
        private PlayerCombat combat;

        private void Awake()
        {
            // Cache component references
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<PlayerCombat>();

            // Initialize states
            idleState = new IdleState(this, movement, combat);
            movingState = new MovingState(this, movement, combat);
            attackingState = new AttackingState(this, movement, combat);

            LogManager.Log("PlayerStateMachine initialized", this);
        }

        private void Start()
        {
            // Start in idle state
            ChangeState(idleState);
        }
    }
}
