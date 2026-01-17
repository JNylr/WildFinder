using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;

namespace Gear.Player
{
    /// <summary>
    /// Handles player combat with server-authoritative damage.
    /// Uses OverlapSphereNonAlloc to avoid allocations in Update loop.
    /// </summary>
    public class PlayerCombat : NetworkBehaviour
    {
        private CharacterStats stats;
        private float lastAttackTime;
        private Transform cachedTransform;

        // Pre-allocated array for physics queries - no allocations in Update()
        private const int MAX_TARGETS = 10;
        private readonly Collider[] hitColliders = new Collider[MAX_TARGETS];

        private void Awake()
        {
            // Cache transform reference
            cachedTransform = transform;
        }

        /// <summary>
        /// Initializes combat with character stats.
        /// Called by PlayerController on network spawn.
        /// </summary>
        /// <param name="characterStats">The stats to use for combat</param>
        public void Initialize(CharacterStats characterStats)
        {
            stats = characterStats;
            LogManager.Log($"PlayerCombat initialized with damage: {stats.AttackDamage}", this);
        }

        private void Update()
        {
            if (!IsOwner) return;

            HandleCombatInput();
        }

        /// <summary>
        /// Processes combat input (attack button).
        /// Called every frame for the owning client.
        /// </summary>
        private void HandleCombatInput()
        {
            // Check for attack input using new Input System
            var keyboard = Keyboard.current;
            var mouse = Mouse.current;
            
            bool attackPressed = (keyboard != null && keyboard.spaceKey.wasPressedThisFrame) ||
                                (mouse != null && mouse.leftButton.wasPressedThisFrame);
            
            if (attackPressed)
            {
                // Check cooldown
                TryAttack();
            }
        }

        /// <summary>
        /// Attempts to perform an attack if cooldown allows.
        /// Called by input handler on the client.
        /// </summary>
        private void TryAttack()
        {
            if (Time.time - lastAttackTime < stats.AttackCooldown)
            {
                LogManager.Log("Attack on cooldown", this);
                return;
            }

            lastAttackTime = Time.time;

            LogManager.Log($"Attempting attack with range: {stats.AttackRange}", this);

            // Find target in range
            Collider[] hitColliders = new Collider[10]; // Pre-allocated array
            int numColliders = Physics.OverlapSphereNonAlloc(
                cachedTransform.position,
                stats.AttackRange,
                hitColliders,
                LayerMask.GetMask(GameConstants.LAYER_ENEMY)
            );

            LogManager.Log($"Found {numColliders} colliders in range", this);

            if (numColliders > 0)
            {
                // Check all colliders to find one with EnemyHealth
                for (int i = 0; i < numColliders; i++)
                {
                    var enemy = hitColliders[i].GetComponent<Enemy.EnemyHealth>();
                    if (enemy != null)
                    {
                        // Trigger attack animation
                        var animController = GetComponentInChildren<PlayerAnimationController>();
                        if (animController != null)
                        {
                            animController.TriggerAttack();
                        }

                        AttackServerRpc(enemy.NetworkObjectId);
                        LogManager.Log($"Attacking enemy at range {Vector3.Distance(cachedTransform.position, hitColliders[i].transform.position)}", this);
                        return; // Found and attacked, exit
                    }
                }
                
                // If we get here, no collider had EnemyHealth
                LogManager.Log("Colliders found but none have EnemyHealth component", this, LogManager.LogLevel.Warning);
            }
            else
            {
                LogManager.Log("No enemies in range", this);
            }
        }

        /// <summary>
        /// Server RPC to process attack damage.
        /// Called by client when attack is performed. Server validates and executes.
        /// </summary>
        /// <param name="targetNetworkId">Network ID of the target enemy</param>
        [Rpc(SendTo.Server)]
        private void AttackServerRpc(ulong targetNetworkId)
        {
            if (!IsServer) return;

            // Server validates the attack
            if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(targetNetworkId, out var targetObject))
            {
                var enemyHealth = targetObject.GetComponent<Enemy.EnemyHealth>();
                if (enemyHealth != null)
                {
                    // Calculate distance on server for validation
                    float distance = Vector3.Distance(cachedTransform.position, targetObject.transform.position);

                    if (distance <= stats.AttackRange)
                    {
                        enemyHealth.TakeDamageServerRpc(stats.AttackDamage);
                        LogManager.Log($"Server: Player dealt {stats.AttackDamage} damage to enemy", this);
                    }
                    else
                    {
                        LogManager.Log($"Server: Attack rejected - target out of range ({distance} > {stats.AttackRange})", this, LogManager.LogLevel.Warning);
                    }
                }
            }
        }

        /// <summary>
        /// Gets whether the player can currently attack.
        /// Called by state machine and UI.
        /// </summary>
        public bool CanAttack => Time.time - lastAttackTime >= stats.AttackCooldown;

        /// <summary>
        /// Gets the attack cooldown progress (0 to 1).
        /// Called by UI to display cooldown indicator.
        /// </summary>
        public float CooldownProgress => Mathf.Clamp01((Time.time - lastAttackTime) / stats.AttackCooldown);
    }
}
