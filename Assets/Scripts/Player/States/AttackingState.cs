using Gear.StateMachine;

namespace Gear.Player.States
{
    /// <summary>
    /// Player attacking state - character is performing an attack.
    /// Transitions back to Idle or Moving based on movement input.
    /// </summary>
    public class AttackingState : IState
    {
        private readonly PlayerStateMachine stateMachine;
        private readonly PlayerMovement movement;
        private readonly PlayerCombat combat;

        private const float ATTACK_DURATION = 0.5f; // Duration of attack animation
        private float attackTimer;

        public AttackingState(PlayerStateMachine sm, PlayerMovement mov, PlayerCombat cbt)
        {
            stateMachine = sm;
            movement = mov;
            combat = cbt;
        }

        public void Enter()
        {
            attackTimer = ATTACK_DURATION;
        }

        public void Update()
        {
            attackTimer -= UnityEngine.Time.deltaTime;

            // Return to appropriate state after attack completes
            if (attackTimer <= 0f)
            {
                if (movement.IsMoving)
                {
                    stateMachine.ChangeState(stateMachine.movingState);
                }
                else
                {
                    stateMachine.ChangeState(stateMachine.idleState);
                }
            }
        }

        public void FixedUpdate()
        {
            // No physics logic during attack
        }

        public void Exit()
        {
            // Attack state exited
        }
    }
}
