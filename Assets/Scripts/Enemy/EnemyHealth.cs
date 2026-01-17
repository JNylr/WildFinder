using UnityEngine;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;

namespace Gear.Enemy
{
    /// <summary>
    /// Enemy health management with NetworkVariable.
    /// Server-authoritative damage handling.
    /// </summary>
    public class EnemyHealth : NetworkBehaviour
    {
        [SerializeField] private CharacterStats stats;

        private NetworkVariable<int> currentHealth = new NetworkVariable<int>(
            100,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        private void Start()
        {
            if (IsServer)
            {
                currentHealth.Value = stats.MaxHealth;
            }

            LogManager.Log($"EnemyHealth initialized: {stats.MaxHealth}", this);
        }

        /// <summary>
        /// Applies damage to the enemy. Server-only.
        /// Called by player combat systems when enemy takes damage.
        /// </summary>
        /// <param name="amount">Damage amount to apply</param>
        [Rpc(SendTo.Server)]
        public void TakeDamageServerRpc(int amount)
        {
            if (!IsServer) return;

            currentHealth.Value = Mathf.Max(0, currentHealth.Value - amount);
            LogManager.Log($"Enemy took {amount} damage. Health: {currentHealth.Value}/{stats.MaxHealth}", this);

            if (currentHealth.Value <= 0)
            {
                Die();
            }
        }

        /// <summary>
        /// Handles enemy death. Server-only.
        /// Called when health reaches zero.
        /// </summary>
        private void Die()
        {
            LogManager.Log("Enemy died!", this, LogManager.LogLevel.Info);

            if (IsServer)
            {
                // Despawn the enemy
                NetworkObject.Despawn(true);
            }
        }

        /// <summary>
        /// Gets current health value.
        /// Called by UI and AI systems.
        /// </summary>
        public int CurrentHealth => currentHealth.Value;

        /// <summary>
        /// Gets maximum health value.
        /// Called by UI to display health bar.
        /// </summary>
        public int MaxHealth => stats.MaxHealth;

        /// <summary>
        /// Gets whether the enemy is alive.
        /// Called by AI and other systems.
        /// </summary>
        public bool IsAlive => currentHealth.Value > 0;
    }
}
