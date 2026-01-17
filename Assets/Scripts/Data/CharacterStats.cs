using UnityEngine;

namespace Gear.Data
{
    /// <summary>
    /// ScriptableObject defining character statistics.
    /// Prevents magic numbers and enables designer-friendly tuning.
    /// </summary>
    [CreateAssetMenu(fileName = "CharacterStats", menuName = "Gear/Character Stats")]
    public class CharacterStats : ScriptableObject
    {
        [Header("Basic Stats")]
        [Tooltip("Maximum health points")]
        public int MaxHealth = 100;

        [Tooltip("Movement speed in units per second")]
        public float MoveSpeed = 5.0f;

        [Tooltip("Rotation speed in degrees per second")]
        public float RotationSpeed = 720.0f;

        [Header("Combat Stats")]
        [Tooltip("Base attack damage")]
        public int AttackDamage = 10;

        [Tooltip("Attack range in units")]
        public float AttackRange = 2.5f;

        [Tooltip("Time between attacks in seconds")]
        public float AttackCooldown = 1.0f;

        [Header("Role-Specific Stats")]
        [Tooltip("Healing power (for Healer role)")]
        public int HealingPower = 15;

        [Tooltip("Damage reduction percentage (for Tank role)")]
        [Range(0f, 1f)]
        public float DamageReduction = 0.0f;

        [Header("Character Info")]
        [Tooltip("Display name of the character")]
        public string CharacterName = "Character";

        [Tooltip("Character role (Tank, Healer, DPS)")]
        public CharacterRole Role = CharacterRole.Tank;
    }

    public enum CharacterRole
    {
        Tank,
        Healer,
        DPS
    }
}
