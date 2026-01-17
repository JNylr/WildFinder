using Gear.StateMachine;

namespace Gear.Player.States
{
    /// <summary>
    /// Player moving state - character is moving.
    /// Transitions to Idle when player stops, or Attacking when player attacks.
    /// </summary>
    public class MovingState : IState
    {
        private readonly PlayerStateMachine stateMachine;
        private readonly PlayerMovement movement;
        private readonly PlayerCombat combat;

        public MovingState(PlayerStateMachine sm, PlayerMovement mov, PlayerCombat cbt)
        {
            stateMachine = sm;
            movement = mov;
            combat = cbt;
        }

        public void Enter()
        {
            // Moving state entered
        }

        public void Update()
        {
            // Check for state transitions
            if (!movement.IsMoving)
            {
                stateMachine.ChangeState(stateMachine.idleState);
            }
        }

        public void FixedUpdate()
        {
            // Movement is handled by PlayerMovement component
        }

        public void Exit()
        {
            // Moving state exited
        }
    }
}
