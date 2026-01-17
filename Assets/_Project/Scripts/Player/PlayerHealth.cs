using UnityEngine;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;

namespace Gear.Player
{
    /// <summary>
    /// Manages player health using NetworkVariable for synchronization.
    /// Server-authoritative damage and healing.
    /// </summary>
    public class PlayerHealth : NetworkBehaviour
    {
        private NetworkVariable<int> currentHealth = new NetworkVariable<int>(
            100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        private int maxHealth;

        /// <summary>
        /// Initializes health with maximum value.
        /// Called by PlayerController on network spawn.
        /// </summary>
        /// <param name="max">Maximum health from CharacterStats</param>
        public void Initialize(int max)
        {
            maxHealth = max;

            if (IsServer)
            {
                currentHealth.Value = maxHealth;
            }

            LogManager.Log($"PlayerHealth initialized: {maxHealth}", this);
        }

        /// <summary>
        /// Applies damage to the player. Server-only.
        /// Called by combat systems when player takes damage.
        /// </summary>
        /// <param name="amount">Damage amount to apply</param>
        [Rpc(SendTo.Server)]
        public void TakeDamageServerRpc(int amount)
        {
            if (!IsServer) return;

            currentHealth.Value = Mathf.Max(0, currentHealth.Value - amount);
            LogManager.Log($"Player took {amount} damage. Health: {currentHealth.Value}/{maxHealth}", this);

            if (currentHealth.Value <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Heals the player. Server-only.
        /// Called by healing abilities.
        /// </summary>
        /// <param name="amount">Healing amount to apply</param>
        [Rpc(SendTo.Server)]
        public void HealServerRpc(int amount)
        {
            if (!IsServer) return;

            currentHealth.Value = Mathf.Min(maxHealth, currentHealth.Value + amount);
            LogManager.Log($"Player healed {amount}. Health: {currentHealth.Value}/{maxHealth}", this);
        }

        /// <summary>
        /// Handles player death. Server-only.
        /// Called when health reaches zero.
        /// </summary>
        private void Die()
        {
            LogManager.Log("Player died!", this, LogManager.LogLevel.Warning);
            // TODO: Implement death behavior (respawn, game over, etc.)
        }

        /// <summary>
        /// Gets current health value.
        /// Called by UI and other systems to display health.
        /// </summary>
        public int CurrentHealth => currentHealth.Value;

        /// <summary>
        /// Gets maximum health value.
        /// Called by UI to display health bar.
        /// </summary>
        public int MaxHealth => maxHealth;

        /// <summary>
        /// Gets whether the player is alive.
        /// Called by state machine and other systems.
        /// </summary>
        public bool IsAlive => currentHealth.Value > 0;
    }
}
