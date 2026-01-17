using UnityEngine;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;

namespace Gear.Enemy
{
    /// <summary>
    /// Main enemy controller coordinating AI and health components.
    /// Server-authoritative enemy behavior.
    /// </summary>
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(EnemyAI))]
    public class EnemyController : NetworkBehaviour
    {
        [SerializeField] private CharacterStats stats;

        private EnemyHealth health;
        private EnemyAI ai;

        private void Awake()
        {
            // Cache component references
            health = GetComponent<EnemyHealth>();
            ai = GetComponent<EnemyAI>();

            LogManager.Log($"EnemyController initialized", this);
        }

        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            LogManager.Log("Enemy spawned on network", this);
        }

        /// <summary>
        /// Gets the enemy's stats.
        /// Called by other systems to query enemy configuration.
        /// </summary>
        public CharacterStats Stats => stats;
    }
}
