# GEAR - Project Architecture

> **Purpose**: This document defines the project structure, coding patterns, and best practices for GEAR. Follow these guidelines to maintain consistency and scalability as the project grows.

---

## Table of Contents

1. [Project Structure](#project-structure)
2. [Folder Organization](#folder-organization)
3. [Naming Conventions](#naming-conventions)
4. [Code Organization](#code-organization)
5. [Scripting Patterns](#scripting-patterns)
6. [Networking Architecture](#networking-architecture)
7. [Asset Management](#asset-management)
8. [Performance Guidelines](#performance-guidelines)

---

## Project Structure

### Root Directory Layout

```
Gear/
├── Assets/
│   ├── _Project/              # All custom game content (underscore sorts to top)
│   ├── Plugins/               # Third-party plugins
│   ├── Settings/              # Unity project settings
│   └── ThirdParty/            # Asset Store packages
├── Docs/                      # Development documentation
├── Packages/                  # Unity Package Manager packages
├── ProjectSettings/           # Unity project configuration
├── .gitignore                 # Git ignore rules
├── README.md                  # Project overview
├── ARCHITECTURE.md            # This file
└── CONTRIBUTING.md            # Contribution guidelines
```

---

## Folder Organization

### Assets/_Project Structure

All custom game content lives under `Assets/_Project/` for clear separation from third-party assets.

```
Assets/_Project/
├── Animations/                # Animation clips and controllers
│   ├── Characters/
│   │   └── PlayerAnimator.controller
│   └── Enemies/
│       └── EnemyAnimator.controller
│
├── Art/                       # Visual assets
│   ├── Materials/             # Material files (.mat)
│   ├── Textures/              # Texture files
│   └── VFX/                   # Visual effects
│
├── Audio/                     # Sound assets
│   ├── Music/                 # Background music
│   ├── SFX/                   # Sound effects
│   └── Mixers/                # Audio mixers
│
├── Data/                      # ScriptableObjects for game data
│   ├── Characters/            # Character stats, abilities
│   │   └── TankSlots.asset
│   └── Enemies/               # Enemy configurations
│       └── BasicEnemyStats.asset
│
├── Models/                    # 3D models and animations
│   ├── Characters/            # Player character models
│   └── Environments/          # Environment props
│
├── Prefabs/                   # Reusable game objects
│   ├── Characters/            # Player prefabs
│   │   └── Player.prefab
│   ├── Enemies/               # Enemy prefabs
│   │   └── Enemy.prefab
│   ├── Environment/           # Environment objects
│   └── UI/                    # UI prefabs
│
├── Scenes/                    # Unity scenes
│   ├── Core/                  # Main game scenes
│   │   └── SampleScene.unity
│   ├── Test/                  # Test/prototype scenes
│   └── Initial.unity          # Bootstrap scene
│
├── Scripts/                   # All C# code (see below)
│
└── UI/                        # UI-specific assets
    ├── Fonts/                 # Font files
    ├── Icons/                 # UI icons and sprites
    └── Prefabs/               # UI prefabs
```

---

### Scripts Organization

```
Assets/_Project/Scripts/
├── Camera/                    # Camera systems
│   └── CameraFollow.cs        # Third-person camera controller
│
├── Combat/                    # Combat-related systems
│   ├── DamageSystem.cs        # Damage calculation
│   ├── HitboxController.cs   # Attack hitboxes
│   └── CombatEffects.cs      # Combat VFX/SFX
│
├── Core/                      # Core game systems
│   ├── GameConstants.cs       # Global constants
│   └── LogManager.cs          # Logging utility
│
├── Data/                      # ScriptableObject definitions
│   └── CharacterStats.cs      # Character stat data structure
│
├── Enemy/                     # Enemy AI and behavior
│   ├── EnemyAI.cs             # AI decision making
│   ├── EnemyController.cs     # Enemy movement/actions
│   └── EnemyHealth.cs         # Enemy health system
│
├── Gameplay/                  # Core gameplay systems
│   ├── ExtractionZone.cs      # Extraction point logic
│   ├── LootSystem.cs          # Loot generation/drops
│   └── GameSession.cs         # Run/session management
│
├── Network/                   # Multiplayer networking
│   └── NetworkUI.cs           # Network lobby UI
│
├── Player/                    # Player systems
│   ├── PlayerController.cs    # Main player controller
│   ├── PlayerMovement.cs      # Movement logic
│   ├── PlayerCombat.cs        # Combat actions
│   ├── PlayerHealth.cs        # Health system
│   ├── PlayerAnimationController.cs  # Animation management
│   └── States/                # Player state machine
│       ├── PlayerStateMachine.cs
│       ├── IdleState.cs
│       ├── MovingState.cs
│       └── AttackingState.cs
│
├── Progression/               # Progression systems
│   ├── BaseManager.cs         # Base building
│   ├── CraftingSystem.cs      # Gear crafting
│   └── UpgradeSystem.cs       # Equipment upgrades
│
├── StateMachine/              # Generic state machine
│   ├── IState.cs              # State interface
│   └── StateMachine.cs        # State machine base
│
└── UI/                        # UI controllers
    ├── HUD/                   # In-game HUD
    ├── Menus/                 # Menu screens
    └── Inventory/             # Inventory UI
```

---

## Naming Conventions

### Files and Folders

| Type | Convention | Example |
|------|------------|---------|
| **Folders** | PascalCase | `Scripts/Player/States/` |
| **C# Scripts** | PascalCase, match class name | `PlayerController.cs` |
| **Scenes** | PascalCase | `MainMenu.unity` |
| **Prefabs** | PascalCase | `Player.prefab` |
| **Materials** | PascalCase with descriptor | `RedEnemy.mat` |
| **ScriptableObjects** | PascalCase with descriptor | `TankSlots.asset` |
| **Animations** | PascalCase | `PlayerIdle.anim` |

### Code Naming

```csharp
// Namespaces: PascalCase, match folder structure
namespace Gear.Player.States { }

// Classes: PascalCase
public class PlayerController : MonoBehaviour { }

// Interfaces: PascalCase with 'I' prefix
public interface IState { }

// Methods: PascalCase
public void TakeDamage(float amount) { }

// Public fields/properties: PascalCase
public float MaxHealth { get; set; }

// Private fields: camelCase with underscore prefix
[SerializeField] private float _moveSpeed = 5f;
private Transform _target;

// Constants: UPPER_SNAKE_CASE
private const float MAX_DISTANCE = 100f;

// Events: PascalCase with 'On' prefix
public event Action OnDeath;
```

---

## Code Organization

### Namespace Structure

All code should use namespaces matching the folder structure:

```csharp
// File: Assets/_Project/Scripts/Player/PlayerController.cs
namespace Gear.Player
{
    public class PlayerController : MonoBehaviour
    {
        // Implementation
    }
}

// File: Assets/_Project/Scripts/Player/States/IdleState.cs
namespace Gear.Player.States
{
    public class IdleState : IState
    {
        // Implementation
    }
}

// File: Assets/_Project/Scripts/Enemy/EnemyAI.cs
namespace Gear.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        // Implementation
    }
}
```

### Assembly Definitions (Future)

As the project grows, add `.asmdef` files for faster compilation:

```
Scripts/
├── Core/
│   └── Gear.Core.asmdef
├── Player/
│   └── Gear.Player.asmdef       # References: Gear.Core
├── Enemy/
│   └── Gear.Enemy.asmdef        # References: Gear.Core
└── Network/
    └── Gear.Network.asmdef      # References: Gear.Core, Gear.Player
```

---

## Scripting Patterns

### MonoBehaviour Organization

Organize MonoBehaviour code in this order:

```csharp
using UnityEngine;
using Unity.Netcode;

namespace Gear.Player
{
    /// <summary>
    /// Controls player movement and input handling.
    /// </summary>
    public class PlayerController : NetworkBehaviour
    {
        // 1. Serialized Fields (Inspector-visible)
        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _rotationSpeed = 10f;

        [Header("References")]
        [SerializeField] private CharacterController _characterController;

        // 2. Public Properties
        public bool IsMoving { get; private set; }

        // 3. Private Fields
        private Vector3 _moveDirection;
        private PlayerStateMachine _stateMachine;

        // 4. Unity Lifecycle Methods
        private void Awake()
        {
            _stateMachine = GetComponent<PlayerStateMachine>();
        }

        private void Start()
        {
            // Initialization
        }

        private void Update()
        {
            if (!IsOwner) return; // Network check
            HandleInput();
        }

        private void FixedUpdate()
        {
            // Physics updates
        }

        // 5. Public Methods
        public void SetMoveSpeed(float speed)
        {
            _moveSpeed = speed;
        }

        // 6. Private Methods
        private void HandleInput()
        {
            // Implementation
        }

        // 7. Network Methods (if applicable)
        public override void OnNetworkSpawn()
        {
            base.OnNetworkSpawn();
            // Network initialization
        }
    }
}
```

### ScriptableObject Pattern

Use ScriptableObjects for game data:

```csharp
using UnityEngine;

namespace Gear.Data
{
    /// <summary>
    /// Defines character statistics and abilities.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Gear/Character Stats")]
    public class CharacterStats : ScriptableObject
    {
        [Header("Base Stats")]
        public float MaxHealth = 100f;
        public float MoveSpeed = 5f;
        public float AttackDamage = 10f;

        [Header("Role")]
        public CharacterRole Role;
        public string[] Abilities;
    }

    public enum CharacterRole
    {
        Tank,
        DPS,
        Healer
    }
}
```

### State Machine Pattern

Use the state machine for complex behaviors:

```csharp
// State interface
namespace Gear.StateMachine
{
    public interface IState
    {
        void Enter();
        void Execute();
        void Exit();
    }
}

// Concrete state
namespace Gear.Player.States
{
    public class IdleState : IState
    {
        private readonly PlayerController _player;

        public IdleState(PlayerController player)
        {
            _player = player;
        }

        public void Enter()
        {
            _player.PlayAnimation("Idle");
        }

        public void Execute()
        {
            // Check for state transitions
        }

        public void Exit()
        {
            // Cleanup
        }
    }
}
```

---

## Networking Architecture

### Unity Netcode for GameObjects

GEAR uses **Unity Netcode for GameObjects** for multiplayer.

#### Key Patterns:

```csharp
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    // Use NetworkVariable for synchronized state
    private NetworkVariable<float> _health = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Owner
    );

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Subscribe to value changes
        _health.OnValueChanged += OnHealthChanged;
    }

    // Use ServerRpc for client → server calls
    [ServerRpc]
    public void TakeDamageServerRpc(float damage)
    {
        _health.Value -= damage;
    }

    // Use ClientRpc for server → client calls
    [ClientRpc]
    private void PlayHitEffectClientRpc()
    {
        // Play VFX on all clients
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        // React to health changes
    }
}
```

#### Network Ownership Rules:

- **Player objects**: Owned by the client controlling them
- **Enemies**: Owned by the server
- **Loot**: Owned by the server until picked up

---

## Asset Management

### Prefab Guidelines

1. **Player Prefabs**: Must have `NetworkObject` component
2. **Enemy Prefabs**: Must be spawnable by server
3. **UI Prefabs**: Should be modular and reusable

### Material Organization

- **Naming**: Descriptive names (e.g., `RedEnemy.mat`, `GrassTerrain.mat`)
- **Location**: `Assets/_Project/Art/Materials/`
- **Shared Materials**: Create material variants for similar objects

### Animation Guidelines

- **Animator Controllers**: One per character type
- **Animation Clips**: Organized by character in `Animations/`
- **Transitions**: Use parameters, not direct connections

---

## Performance Guidelines

### General Rules

1. **Avoid `Update()` when possible**: Use events or coroutines
2. **Cache component references**: Don't call `GetComponent()` every frame
3. **Object pooling**: For frequently spawned objects (projectiles, VFX)
4. **LOD for models**: Use Level of Detail for distant objects
5. **Occlusion culling**: Enable for large environments

### Example: Component Caching

```csharp
// ❌ Bad - calls GetComponent every frame
void Update()
{
    GetComponent<Rigidbody>().AddForce(Vector3.up);
}

// ✅ Good - cache in Awake
private Rigidbody _rigidbody;

void Awake()
{
    _rigidbody = GetComponent<Rigidbody>();
}

void Update()
{
    _rigidbody.AddForce(Vector3.up);
}
```

### Network Performance

- Minimize `NetworkVariable` updates
- Use `[ServerRpc]` only when necessary
- Batch network calls when possible

---

## Quick Reference

### Adding a New Feature

1. **Determine category**: Player, Enemy, Gameplay, etc.
2. **Create script** in appropriate `Scripts/` subfolder
3. **Use correct namespace**: Match folder structure
4. **Follow naming conventions**: PascalCase for classes/files
5. **Add XML documentation**: `/// <summary>` comments
6. **Create prefab** if needed in `Prefabs/` subfolder
7. **Update this document** if adding new patterns

### Common Locations

| Asset Type | Location |
|------------|----------|
| Player scripts | `Scripts/Player/` |
| Enemy scripts | `Scripts/Enemy/` |
| ScriptableObjects | `Data/` + `Scripts/Data/` |
| Prefabs | `Prefabs/[Category]/` |
| Scenes | `Scenes/Core/` or `Scenes/Test/` |
| Materials | `Art/Materials/` |
| UI assets | `UI/` |

---

## For AI Assistants

When helping with this project:

1. **Follow this architecture**: All new code should match these patterns
2. **Use correct namespaces**: Match the folder structure
3. **Maintain naming conventions**: PascalCase, camelCase as specified
4. **Add documentation**: XML comments for public APIs
5. **Consider networking**: Use `NetworkBehaviour` for multiplayer objects
6. **Reference this document**: When unsure about structure

---

**Last Updated**: 2026-01-17  
**Version**: 1.0
