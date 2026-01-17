using Gear.StateMachine;

namespace Gear.Player.States
{
    /// <summary>
    /// Player idle state - no movement or combat.
    /// Transitions to Moving when player moves, or Attacking when player attacks.
    /// </summary>
    public class IdleState : IState
    {
        private readonly PlayerStateMachine stateMachine;
        private readonly PlayerMovement movement;
        private readonly PlayerCombat combat;

        public IdleState(PlayerStateMachine sm, PlayerMovement mov, PlayerCombat cbt)
        {
            stateMachine = sm;
            movement = mov;
            combat = cbt;
        }

        public void Enter()
        {
            // Idle state entered
        }

        public void Update()
        {
            // Check for state transitions
            if (movement.IsMoving)
            {
                stateMachine.ChangeState(stateMachine.movingState);
            }
        }

        public void FixedUpdate()
        {
            // No physics logic in idle state
        }

        public void Exit()
        {
            // Idle state exited
        }
    }
}
