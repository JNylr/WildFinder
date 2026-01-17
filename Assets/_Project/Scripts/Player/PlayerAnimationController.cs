using UnityEngine;
using Unity.Netcode;
using Gear.Core;

namespace Gear.Player
{
    /// <summary>
    /// Controls player character animations based on movement and combat state.
    /// Updates Animator parameters to trigger animation transitions.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimationController : NetworkBehaviour
    {
        private Animator animator;
        private PlayerMovement playerMovement;
        private PlayerCombat playerCombat;

        // Animation parameter hashes (more efficient than strings)
        private static readonly int IsMovingHash = Animator.StringToHash("isMoving");
        private static readonly int AttackHash = Animator.StringToHash("attack");

        private void Awake()
        {
            animator = GetComponent<Animator>();
            
            // Get references from parent Player GameObject
            playerMovement = GetComponentInParent<PlayerMovement>();
            playerCombat = GetComponentInParent<PlayerCombat>();

            LogManager.Log($"PlayerAnimationController initialized. Animator: {animator != null}, Movement: {playerMovement != null}, Combat: {playerCombat != null}", this);
        }

        private void Update()
        {
            if (!IsOwner) return;

            // Update movement animation
            if (playerMovement != null && animator != null)
            {
                bool isMoving = playerMovement.IsMoving;
                animator.SetBool(IsMovingHash, isMoving);
                
                // Debug log occasionally
                if (Time.frameCount % 60 == 0)
                {
                    LogManager.Log($"Animation update: isMoving={isMoving}", this);
                }
            }
        }

        /// <summary>
        /// Triggers the attack animation.
        /// Called by PlayerCombat when attack is performed.
        /// </summary>
        public void TriggerAttack()
        {
            if (animator != null)
            {
                animator.SetTrigger(AttackHash);
                LogManager.Log("Attack animation triggered", this);
            }
        }
    }
}
