# Contributing to GEAR

This guide helps developers (human and AI) contribute to GEAR following established patterns and best practices.

## Quick Start

1. **Read [ARCHITECTURE.md](ARCHITECTURE.md)** - Understand project structure
2. **Follow naming conventions** - PascalCase for classes, camelCase for private fields
3. **Use correct namespaces** - Match folder structure (`Gear.Player`, `Gear.Enemy`, etc.)
4. **Add XML documentation** - Document public APIs
5. **Test your changes** - Verify in Unity Play mode

---

## Adding New Features

### 1. Determine Feature Category

Place your code in the appropriate folder:

| Feature Type | Folder | Namespace |
|--------------|--------|-----------|
| Player abilities | `Scripts/Player/` | `Gear.Player` |
| Enemy AI | `Scripts/Enemy/` | `Gear.Enemy` |
| Combat systems | `Scripts/Combat/` | `Gear.Combat` |
| Loot/extraction | `Scripts/Gameplay/` | `Gear.Gameplay` |
| Base building | `Scripts/Progression/` | `Gear.Progression` |
| UI controllers | `Scripts/UI/` | `Gear.UI` |
| Core utilities | `Scripts/Core/` | `Gear.Core` |

### 2. Create Script Template

```csharp
using UnityEngine;
using Unity.Netcode; // If networking needed

namespace Gear.[Category]
{
    /// <summary>
    /// Brief description of what this class does.
    /// </summary>
    public class YourClassName : MonoBehaviour // or NetworkBehaviour
    {
        #region Serialized Fields
        [Header("Settings")]
        [SerializeField] private float _exampleValue = 1f;

        [Header("References")]
        [SerializeField] private Transform _target;
        #endregion

        #region Public Properties
        public bool IsActive { get; private set; }
        #endregion

        #region Private Fields
        private bool _isInitialized;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            // Component initialization
        }

        private void Start()
        {
            // Game initialization
        }

        private void Update()
        {
            // Per-frame updates
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Public method description.
        /// </summary>
        public void DoSomething()
        {
            // Implementation
        }
        #endregion

        #region Private Methods
        private void HelperMethod()
        {
            // Implementation
        }
        #endregion
    }
}
```

### 3. Create Prefab (if needed)

1. Create GameObject in scene
2. Add your script component
3. Configure in Inspector
4. Drag to `Assets/_Project/Prefabs/[Category]/`
5. Delete from scene

### 4. Create ScriptableObject (for data)

```csharp
using UnityEngine;

namespace Gear.Data
{
    [CreateAssetMenu(fileName = "NewData", menuName = "Gear/Your Data Type")]
    public class YourData : ScriptableObject
    {
        [Header("Configuration")]
        public float Value;
        public string Name;
    }
}
```

Then create asset: `Right-click in Project → Create → Gear → Your Data Type`

---

## Networking Guidelines

### When to Use NetworkBehaviour

Use `NetworkBehaviour` for:
- ✅ Player-controlled objects
- ✅ Enemies (server-controlled)
- ✅ Loot items
- ✅ Anything that needs synchronization

Use regular `MonoBehaviour` for:
- ✅ UI controllers
- ✅ Camera systems
- ✅ Local effects
- ✅ Utilities

### Network Patterns

```csharp
using Unity.Netcode;

public class NetworkedObject : NetworkBehaviour
{
    // Synchronized variable
    private NetworkVariable<float> _health = new NetworkVariable<float>(
        100f,
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server // or Owner
    );

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        // Only run on owner
        if (IsOwner)
        {
            // Local player initialization
        }

        // Only run on server
        if (IsServer)
        {
            // Server-side initialization
        }

        // Subscribe to network variable changes
        _health.OnValueChanged += OnHealthChanged;
    }

    // Client calls this, server executes
    [ServerRpc]
    public void TakeDamageServerRpc(float damage)
    {
        if (!IsServer) return;
        _health.Value -= damage;
    }

    // Server calls this, all clients execute
    [ClientRpc]
    private void PlayEffectClientRpc()
    {
        // Play VFX/SFX on all clients
    }

    private void OnHealthChanged(float oldValue, float newValue)
    {
        // React to health changes
    }
}
```

---

## Code Style

### Naming Conventions

```csharp
// Classes, Methods, Properties: PascalCase
public class PlayerController { }
public void MovePlayer() { }
public float MaxHealth { get; set; }

// Private fields: _camelCase (underscore prefix)
private float _moveSpeed;
[SerializeField] private Transform _target;

// Constants: UPPER_SNAKE_CASE
private const float MAX_DISTANCE = 100f;

// Interfaces: IPascalCase
public interface IState { }

// Enums: PascalCase
public enum CharacterRole { Tank, DPS, Healer }
```

### Documentation

Add XML comments to public APIs:

```csharp
/// <summary>
/// Applies damage to the character.
/// </summary>
/// <param name="amount">Amount of damage to apply.</param>
/// <param name="source">Source of the damage.</param>
/// <returns>True if the character died from this damage.</returns>
public bool TakeDamage(float amount, GameObject source)
{
    // Implementation
}
```

### Performance Best Practices

```csharp
// ✅ Cache component references
private Rigidbody _rigidbody;

private void Awake()
{
    _rigidbody = GetComponent<Rigidbody>();
}

// ❌ Don't call GetComponent every frame
private void Update()
{
    GetComponent<Rigidbody>().AddForce(Vector3.up); // BAD!
}

// ✅ Use events instead of polling
public event Action OnDeath;

private void Die()
{
    OnDeath?.Invoke();
}

// ❌ Don't check conditions every frame
private void Update()
{
    if (health <= 0) Die(); // BAD if health rarely changes
}
```

---

## State Machine Pattern

For complex behaviors (player, enemy AI), use the state machine:

### 1. Create State Interface Implementation

```csharp
namespace Gear.Player.States
{
    public class NewState : IState
    {
        private readonly PlayerController _player;

        public NewState(PlayerController player)
        {
            _player = player;
        }

        public void Enter()
        {
            // Called when entering this state
            _player.PlayAnimation("StateName");
        }

        public void Execute()
        {
            // Called every frame while in this state
            // Check for transitions to other states
        }

        public void Exit()
        {
            // Called when leaving this state
            // Cleanup
        }
    }
}
```

### 2. Register State in StateMachine

```csharp
// In PlayerStateMachine.cs
private void Awake()
{
    _states.Add("NewState", new NewState(_player));
}
```

---

## Testing Your Changes

### In Unity Editor

1. **Enter Play Mode**: Click Play button
2. **Test core functionality**:
   - Player movement (WASD)
   - Combat (Left Click)
   - Camera follows player
3. **Check Console**: No errors or warnings
4. **Test multiplayer** (if applicable):
   - Start as Host
   - Test with multiple clients

### Common Issues

| Issue | Solution |
|-------|----------|
| Missing references | Assign in Inspector or use `GetComponent` |
| NullReferenceException | Check initialization order (Awake vs Start) |
| Network not syncing | Ensure using `NetworkVariable` or RPCs |
| Animation not playing | Check Animator parameters and transitions |

---

## Git Workflow

### Committing Changes

```bash
# Check what changed
git status

# Add specific files
git add Assets/_Project/Scripts/Player/NewFeature.cs

# Or add all changes
git add .

# Commit with descriptive message
git commit -m "Add new player ability: Dash

- Created DashAbility.cs
- Added dash animation
- Integrated with PlayerController"

# Push to remote
git push
```

### Commit Message Format

```
<Type>: <Short description>

<Detailed description>
- Bullet point 1
- Bullet point 2
```

**Types**: `Add`, `Fix`, `Refactor`, `Update`, `Remove`

**Examples**:
- `Add: Player dash ability`
- `Fix: Camera clipping through walls`
- `Refactor: Improve enemy AI pathfinding`
- `Update: Increase player movement speed`

---

## For AI Assistants

When assisting with this project:

### Always Do:
- ✅ Read `ARCHITECTURE.md` before making changes
- ✅ Use correct namespaces matching folder structure
- ✅ Follow naming conventions (PascalCase, _camelCase)
- ✅ Add XML documentation to public methods
- ✅ Consider networking implications (use `NetworkBehaviour` when needed)
- ✅ Cache component references in `Awake()`
- ✅ Use `#region` for code organization

### Never Do:
- ❌ Create scripts without proper namespace
- ❌ Use `GetComponent()` in `Update()`
- ❌ Ignore networking for multiplayer features
- ❌ Create prefabs in wrong folders
- ❌ Skip documentation for public APIs

### When Unsure:
1. Check existing similar code
2. Reference `ARCHITECTURE.md`
3. Ask the user for clarification

---

## Common Tasks

### Adding a New Enemy Type

1. Create `Scripts/Enemy/NewEnemyAI.cs`
2. Inherit from `EnemyController` or create new class
3. Create ScriptableObject in `Data/Enemies/NewEnemyStats.asset`
4. Create prefab in `Prefabs/Enemies/NewEnemy.prefab`
5. Add to spawn system

### Adding a New Player Ability

1. Create `Scripts/Player/NewAbility.cs`
2. Add to `PlayerCombat.cs` or create new state
3. Create animation clip in `Animations/Characters/`
4. Add animator parameter and transition
5. Test in Play mode

### Adding UI Screen

1. Create prefab in `Prefabs/UI/NewScreen.prefab`
2. Create controller in `Scripts/UI/NewScreenController.cs`
3. Use Canvas for UI layout
4. Hook up button events

---

## Questions?

- Check `ARCHITECTURE.md` for structure guidelines
- Check `README.md` for project overview
- Check `TROUBLESHOOTING.md` for common issues

---

**Last Updated**: 2026-01-17
