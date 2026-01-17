# Coding Guidelines - Medieval Fantasy ARPG

## Project Context: Reliable Multiplayer ARPG
- **Engine:** Unity 6 (LTS)
- **Render Pipeline:** Universal Render Pipeline (URP) - Target 60fps on Steam Deck
- **Networking:** Unity Netcode for GameObjects (NGO) + Steamworks.NET
- **Architecture:** Server-Authoritative (ServerAuth)
- **Language:** C# (Latest supported version)

---

## 1. Reliability Guidelines (The "App Dev" Rules)

### Zero Magic Numbers
- **Rule:** All gameplay values (speed, damage, health, cooldowns, ranges) MUST be defined in `ScriptableObjects` or `const` variables
- **Rationale:** Enables designer-friendly tuning, prevents bugs from scattered hardcoded values
- **Example:**
  ```csharp
  // ❌ BAD
  transform.position += Vector3.forward * 5.5f * Time.deltaTime;
  
  // ✅ GOOD
  [SerializeField] private CharacterStats stats;
  transform.position += Vector3.forward * stats.MoveSpeed * Time.deltaTime;
  ```

### State Management
- **Rule:** Use the State Pattern for all entity logic (Player, Enemies, NPCs)
- **Rationale:** Prevents giant `switch` statements, makes behavior testable and modular
- **Structure:**
  ```
  Assets/Scripts/StateMachine/
    ├── IState.cs
    ├── StateMachine.cs
    └── [Entity]States/
        ├── IdleState.cs
        ├── MovingState.cs
        └── AttackingState.cs
  ```
- **Anti-pattern:** Do NOT use `if (isIdle) ... else if (isMoving) ... else if (isAttacking)` in `Update()`

### Memory Safety
- **Rule:** Avoid `new` keyword in `Update()`, `FixedUpdate()`, or any per-frame method
- **Rationale:** Prevents garbage collection spikes that cause frame drops
- **Best Practices:**
  - Cache all references in `Awake()`
  - Use object pooling for frequently instantiated objects (projectiles, VFX)
  - Pre-allocate collections with known sizes
  ```csharp
  // ❌ BAD
  void Update() {
      var enemies = new List<Enemy>();
      // ...
  }
  
  // ✅ GOOD
  private List<Enemy> cachedEnemies;
  void Awake() {
      cachedEnemies = new List<Enemy>(10);
  }
  ```

### Networking Rules
- **Rule:** All combat logic occurs on the Server (`[ServerRpc]`)
- **Rule:** Clients only send inputs, never results
- **Rule:** Use `NetworkVariable<T>` for syncing state
- **Example:**
  ```csharp
  // Client sends input
  [ServerRpc]
  private void AttackServerRpc(Vector3 targetPosition) {
      // Server validates and executes
      if (CanAttack()) {
          DealDamage(targetPosition);
      }
  }
  
  // Server syncs result
  private NetworkVariable<int> health = new NetworkVariable<int>();
  ```

---

## 2. Intel Mac & Steam Deck Optimization

### Shader Stripping
- **Rule:** Do NOT include complex HDRP shaders
- **Rule:** Use simple URP Lit or Unlit shaders only
- **Rationale:** Reduces shader compilation time and improves performance on lower-end GPUs

### Physics Optimization
- **Rule:** Run physics checks ONLY in `FixedUpdate()`
- **Rule:** Use layer-based collision filtering
- **Example:**
  ```csharp
  void FixedUpdate() {
      // ✅ Physics checks here
      if (Physics.Raycast(...)) { }
  }
  
  void Update() {
      // ❌ NO physics checks here
  }
  ```

### Logging Performance
- **Rule:** Wrap ALL `Debug.Log` calls in a custom `LogManager`
- **Rationale:** Logs can be stripped from Release builds for significant performance gains
- **Usage:**
  ```csharp
  // ❌ BAD
  Debug.Log("Player health: " + health);
  
  // ✅ GOOD
  LogManager.Log("Player health: " + health, LogLevel.Info);
  ```

---

## 3. Code Style (SOLID Principles)

### Single Responsibility Principle
- **Rule:** If a script does two things, split it
- **Example:**
  - `PlayerMovement.cs` - Only handles movement
  - `PlayerHealth.cs` - Only handles health/damage
  - `PlayerCombat.cs` - Only handles attacks
  - `PlayerController.cs` - Coordinates the above components

### Open/Closed Principle
- **Rule:** Use inheritance and interfaces for extensibility
- **Example:** Different character classes (Tank, Healer, DPS) inherit from `CharacterBase`

### Liskov Substitution Principle
- **Rule:** Derived classes must be substitutable for base classes
- **Example:** Any `IState` implementation can be used in `StateMachine`

### Interface Segregation Principle
- **Rule:** Create focused interfaces, not monolithic ones
- **Example:** `IDamageable`, `IHealable`, `IInteractable` instead of one `IEntity`

### Dependency Inversion Principle
- **Rule:** Depend on abstractions, not concrete implementations
- **Example:** `PlayerCombat` depends on `IDamageable`, not `EnemyHealth`

### Formatting
- **Brace Style:** K&R (opening brace on same line)
  ```csharp
  public void Attack() {
      if (CanAttack()) {
          DealDamage();
      }
  }
  ```
- **Naming:**
  - `PascalCase` for public members, methods, classes
  - `camelCase` for private fields (with underscore prefix optional)
  - `UPPER_CASE` for constants
- **Spacing:** 4 spaces for indentation (no tabs)

### XML Documentation
- **Rule:** Add `///` summary above EVERY public method
- **Required Info:**
  - What the method does
  - Who calls it (Server/Client/Both)
  - Any important side effects
- **Example:**
  ```csharp
  /// <summary>
  /// Deals damage to the target. Server-only.
  /// Called by PlayerCombat when attack input is received.
  /// </summary>
  /// <param name="target">The entity to damage</param>
  /// <param name="amount">Damage amount from CharacterStats</param>
  [ServerRpc]
  public void DealDamageServerRpc(ulong targetId, int amount) {
      // ...
  }
  ```

---

## 4. Agent Boundaries (DO NOT DO)

### Project Settings
- **Rule:** DO NOT modify `ProjectSettings` without explicit user approval
- **Examples:** Quality settings, Physics layers, Input system settings

### Singleton Managers
- **Rule:** DO NOT create "Manager" singletons that survive scene loads unless explicitly instructed
- **Rationale:** Can cause hard-to-debug state issues in multiplayer
- **Exception:** Core systems like `LogManager` or `NetworkManager` (Unity's built-in)

### Performance Anti-Patterns
- **Rule:** DO NOT use `GameObject.Find()` or `GetComponent()` inside `Update()`
- **Rule:** Cache all component references in `Awake()` or `Start()`
- **Example:**
  ```csharp
  // ❌ BAD
  void Update() {
      GetComponent<Rigidbody>().velocity = Vector3.zero;
  }
  
  // ✅ GOOD
  private Rigidbody rb;
  void Awake() {
      rb = GetComponent<Rigidbody>();
  }
  void Update() {
      rb.velocity = Vector3.zero;
  }
  ```

---

## 5. Project Structure

```
Assets/
├── Data/                          # ScriptableObjects
│   ├── Characters/
│   │   ├── TankStats.asset
│   │   └── HealerStats.asset
│   └── Enemies/
│       └── BasicEnemyStats.asset
├── Prefabs/
│   ├── Player/
│   └── Enemies/
├── Scenes/
│   └── GameScene.unity
├── Scripts/
│   ├── Core/
│   │   ├── LogManager.cs
│   │   └── GameConstants.cs
│   ├── Data/
│   │   └── CharacterStats.cs
│   ├── StateMachine/
│   │   ├── IState.cs
│   │   └── StateMachine.cs
│   ├── Player/
│   │   ├── PlayerController.cs
│   │   ├── PlayerMovement.cs
│   │   ├── PlayerCombat.cs
│   │   ├── PlayerHealth.cs
│   │   └── States/
│   └── Enemy/
│       ├── EnemyController.cs
│       ├── EnemyHealth.cs
│       └── EnemyAI.cs
└── Shaders/
    └── URP/                       # Only URP shaders
```

---

## 6. Checklist for New Features

Before implementing any new feature, verify:
- [ ] All values are in ScriptableObjects or constants (no magic numbers)
- [ ] State machine is used for complex behavior (no giant switch statements)
- [ ] No allocations in Update/FixedUpdate
- [ ] Server-authoritative for all gameplay logic
- [ ] All public methods have XML documentation
- [ ] Component references cached in Awake()
- [ ] Physics checks only in FixedUpdate()
- [ ] Debug.Log wrapped in LogManager
- [ ] Follows SOLID principles
- [ ] K&R brace style used

---

## 7. Common Patterns

### Character Setup Pattern
```csharp
public class PlayerController : NetworkBehaviour {
    [SerializeField] private CharacterStats stats;
    
    // Cached references
    private PlayerMovement movement;
    private PlayerCombat combat;
    private PlayerHealth health;
    
    private void Awake() {
        // Cache all references
        movement = GetComponent<PlayerMovement>();
        combat = GetComponent<PlayerCombat>();
        health = GetComponent<PlayerHealth>();
    }
    
    private void Start() {
        // Initialize with ScriptableObject data
        health.Initialize(stats.MaxHealth);
        movement.Initialize(stats.MoveSpeed);
    }
}
```

### State Machine Pattern
```csharp
public class PlayerStateMachine : StateMachine {
    public IdleState idleState;
    public MovingState movingState;
    public AttackingState attackingState;
    
    private void Awake() {
        idleState = new IdleState(this);
        movingState = new MovingState(this);
        attackingState = new AttackingState(this);
        
        ChangeState(idleState);
    }
}
```

### Network Combat Pattern
```csharp
public class PlayerCombat : NetworkBehaviour {
    [SerializeField] private CharacterStats stats;
    
    /// <summary>
    /// Client calls this to request an attack. Server validates and executes.
    /// </summary>
    [ServerRpc]
    private void AttackServerRpc(ulong targetId) {
        if (!CanAttack()) return;
        
        if (NetworkManager.SpawnManager.SpawnedObjects.TryGetValue(targetId, out var targetObject)) {
            if (targetObject.TryGetComponent<IDamageable>(out var damageable)) {
                damageable.TakeDamage(stats.AttackDamage);
            }
        }
    }
}
```

---

## Summary

These guidelines ensure:
- **Reliability:** No magic numbers, predictable state management
- **Performance:** No GC spikes, optimized for Steam Deck
- **Maintainability:** SOLID principles, clear documentation
- **Multiplayer-Ready:** Server-authoritative architecture from day one

Follow these rules strictly to build a robust, scalable ARPG.
