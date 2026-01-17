using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;

namespace Gear.Player
{
    /// <summary>
    /// Handles player movement with CharacterController.
    /// Uses ScriptableObject for all movement values - no magic numbers.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        private CharacterStats stats;
        private CharacterController characterController;
        private Vector3 moveDirection;
        private float currentSpeed;

        private const float GRAVITY = -9.81f;
        private float verticalVelocity;

        private void Awake()
        {
            // Cache CharacterController reference
            characterController = GetComponent<CharacterController>();
        }

        /// <summary>
        /// Initializes movement with character stats.
        /// Called by PlayerController on network spawn.
        /// </summary>
        /// <param name="characterStats">The stats to use for movement</param>
        public void Initialize(CharacterStats characterStats)
        {
            stats = characterStats;
            LogManager.Log($"PlayerMovement initialized with speed: {stats.MoveSpeed}", this);
        }

        private void Update()
        {
            if (!IsOwner) return;

            HandleMovement();
            HandleRotation();
        }

        /// <summary>
        /// Processes movement input and applies to CharacterController.
        /// Called every frame for the owning client.
        /// </summary>
        private void HandleMovement()
        {
            // Get input from new Input System using Keyboard
            var keyboard = Keyboard.current;
            if (keyboard == null) return;

            // Safety check for stats
            if (stats == null)
            {
                LogManager.Log("PlayerMovement: stats is null! Make sure CharacterStats is assigned in PlayerController.", this, LogManager.LogLevel.Error);
                return;
            }

            float horizontal = 0f;
            float vertical = 0f;

            if (keyboard.aKey.isPressed) horizontal -= 1f;
            if (keyboard.dKey.isPressed) horizontal += 1f;
            if (keyboard.wKey.isPressed) vertical += 1f;
            if (keyboard.sKey.isPressed) vertical -= 1f;

            // Calculate move direction
            moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

            // Apply movement
            if (moveDirection.magnitude >= 0.1f)
            {
                Vector3 move = moveDirection * stats.MoveSpeed * Time.deltaTime;
                characterController.Move(move);
                currentSpeed = stats.MoveSpeed;
            }
            else
            {
                currentSpeed = 0f;
            }

            // Apply gravity
            if (characterController.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; // Small downward force to keep grounded
            }
            else
            {
                verticalVelocity += GRAVITY * Time.deltaTime;
            }

            characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }

        /// <summary>
        /// Rotates player to face movement direction.
        /// Called every frame for the owning client.
        /// </summary>
        private void HandleRotation()
        {
            if (moveDirection.magnitude >= 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                transform.rotation = Quaternion.RotateTowards(
                    transform.rotation,
                    targetRotation,
                    stats.RotationSpeed * Time.deltaTime
                );
            }
        }

        /// <summary>
        /// Gets the current movement speed.
        /// Called by state machine to determine state transitions.
        /// </summary>
        public float CurrentSpeed => currentSpeed;

        /// <summary>
        /// Gets whether the player is moving.
        /// Called by state machine and animation controller.
        /// </summary>
        public bool IsMoving => currentSpeed > 0.1f;
    }
}
