using UnityEngine;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;
using Gear.Player.States;

namespace Gear.Player
{
    /// <summary>
    /// Main player controller handling input and component coordination.
    /// Caches all references in Awake() for performance.
    /// </summary>
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerCombat))]
    [RequireComponent(typeof(PlayerHealth))]
    public class PlayerController : NetworkBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private CharacterStats stats;

        // Cached component references
        private PlayerMovement movement;
        private PlayerCombat combat;
        private PlayerHealth health;
        private PlayerStateMachine stateMachine;

        private void Awake()
        {
            // Cache all component references - never use GetComponent in Update()
            movement = GetComponent<PlayerMovement>();
            combat = GetComponent<PlayerCombat>();
            health = GetComponent<PlayerHealth>();
            stateMachine = GetComponent<PlayerStateMachine>();

            LogManager.Log($"PlayerController initialized for {stats.CharacterName}", this);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();

            if (!IsOwner)
            {
                // Disable input for non-owned players
                enabled = false;
                return;
            }

            // Initialize components with ScriptableObject data
            health.Initialize(stats.MaxHealth);
            movement.Initialize(stats);
            combat.Initialize(stats);

            LogManager.Log($"Player spawned as {stats.Role}", this);
        }

        private void Update()
        {
            if (!IsOwner) return;

            // Handle input - components will read from Input System
            HandleInput();
        }

        /// <summary>
        /// Processes player input and delegates to appropriate components.
        /// Called every frame for the owning client only.
        /// </summary>
        private void HandleInput()
        {
            // Movement is handled by PlayerMovement component
            // Combat is handled by PlayerCombat component
            // State machine coordinates behavior
        }

        /// <summary>
        /// Gets the player's current stats.
        /// Called by other systems to query player configuration.
        /// </summary>
        public CharacterStats Stats => stats;
    }
}
