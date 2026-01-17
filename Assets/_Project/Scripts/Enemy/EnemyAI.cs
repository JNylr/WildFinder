using UnityEngine;
using Unity.Netcode;
using Gear.Data;
using Gear.Core;

namespace Gear.Enemy
{
    /// <summary>
    /// Basic AI for enemy behavior (idle, patrol, chase).
    /// Server-authoritative movement and decision making.
    /// </summary>
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyAI : NetworkBehaviour
    {
        [SerializeField] private CharacterStats stats;
        [SerializeField] private float detectionRange = 10f;
        [SerializeField] private float attackRange = 2.5f;

        private EnemyHealth health;
        private Transform cachedTransform;
        private Transform targetPlayer;
        private Vector3 startPosition;

        private const float PATROL_RADIUS = 5f;
        private const float PATROL_WAIT_TIME = 2f;
        private float patrolTimer;

        private void Awake()
        {
            // Cache references
            health = GetComponent<EnemyHealth>();
            cachedTransform = transform;
        }

        private void Start()
        {
            startPosition = cachedTransform.position;
        }

        private void Update()
        {
            // Only server runs AI logic
            if (!IsServer) return;
            if (!health.IsAlive) return;

            UpdateAI();
        }

        /// <summary>
        /// Updates AI behavior. Server-only.
        /// Called every frame on the server.
        /// </summary>
        private void UpdateAI()
        {
            // Find nearest player
            targetPlayer = FindNearestPlayer();

            if (targetPlayer != null)
            {
                float distanceToPlayer = Vector3.Distance(cachedTransform.position, targetPlayer.position);

                if (distanceToPlayer <= attackRange)
                {
                    // Attack player
                    AttackPlayer();
                }
                else if (distanceToPlayer <= detectionRange)
                {
                    // Chase player
                    ChasePlayer();
                }
                else
                {
                    // Patrol
                    Patrol();
                }
            }
            else
            {
                // No players nearby, patrol
                Patrol();
            }
        }

        /// <summary>
        /// Finds the nearest player within detection range. Server-only.
        /// Called by AI update logic.
        /// </summary>
        private Transform FindNearestPlayer()
        {
            Collider[] hitColliders = new Collider[10]; // Pre-allocated array
            int numColliders = Physics.OverlapSphereNonAlloc(
                cachedTransform.position,
                detectionRange,
                hitColliders,
                LayerMask.GetMask(GameConstants.LAYER_PLAYER)
            );

            Transform nearest = null;
            float nearestDistance = float.MaxValue;

            for (int i = 0; i < numColliders; i++)
            {
                float distance = Vector3.Distance(cachedTransform.position, hitColliders[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearest = hitColliders[i].transform;
                }
            }

            return nearest;
        }

        /// <summary>
        /// Moves enemy towards the player. Server-only.
        /// Called when player is in detection range.
        /// </summary>
        private void ChasePlayer()
        {
            Vector3 direction = (targetPlayer.position - cachedTransform.position).normalized;
            cachedTransform.position += direction * stats.MoveSpeed * Time.deltaTime;

            // Rotate to face player
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            cachedTransform.rotation = Quaternion.RotateTowards(
                cachedTransform.rotation,
                targetRotation,
                stats.RotationSpeed * Time.deltaTime
            );
        }

        /// <summary>
        /// Attacks the player. Server-only.
        /// Called when player is in attack range.
        /// </summary>
        private void AttackPlayer()
        {
            // Face the player
            Vector3 direction = (targetPlayer.position - cachedTransform.position).normalized;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            cachedTransform.rotation = targetRotation;

            // TODO: Implement attack cooldown and damage dealing
            // For now, just log the attack
            // LogManager.Log("Enemy attacking player!", this); // Commented out - too spammy
        }

        /// <summary>
        /// Patrols around the start position. Server-only.
        /// Called when no players are nearby.
        /// </summary>
        private void Patrol()
        {
            patrolTimer -= Time.deltaTime;

            if (patrolTimer <= 0f)
            {
                // Pick a new random patrol point
                Vector3 randomPoint = startPosition + Random.insideUnitSphere * PATROL_RADIUS;
                randomPoint.y = startPosition.y; // Keep same height

                // Move towards patrol point
                Vector3 direction = (randomPoint - cachedTransform.position).normalized;
                cachedTransform.position += direction * (stats.MoveSpeed * 0.5f) * Time.deltaTime;

                // Reset patrol timer
                patrolTimer = PATROL_WAIT_TIME;
            }
        }

        private void OnDrawGizmosSelected()
        {
            // Visualize detection and attack ranges
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
