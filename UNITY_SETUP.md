# Unity Setup Guide - Getting Started

## Current Issue: Scripts Not Visible in Unity

Unity has created the script files, but you need to set up the project properly. Follow these steps:

## Step 1: Install Required Packages

1. Open **Window > Package Manager**
2. Click the **+** button (top-left)
3. Select **Add package by name...**
4. Add these packages one at a time:
   - `com.unity.netcode.gameobjects`
   - `com.unity.inputsystem` (if not already installed)

5. Wait for Unity to compile after each package installation

## Step 2: Fix ScriptableObject References

The `.asset` files I created need to be recreated through Unity's interface:

### Create Tank Stats:
1. In Project window, navigate to `Assets/Data/Characters/`
2. Delete the existing `TankStats.asset` file
3. Right-click in the folder > **Create > Gear > Character Stats**
4. Name it `TankStats`
5. Select it and set these values in Inspector:
   - Max Health: `200`
   - Move Speed: `4.0`
   - Rotation Speed: `540.0`
   - Attack Damage: `15`
   - Attack Range: `2.5`
   - Attack Cooldown: `1.2`
   - Healing Power: `0`
   - Damage Reduction: `0.3`
   - Character Name: `Tank`
   - Role: `Tank`

### Create Healer Stats:
1. In `Assets/Data/Characters/`
2. Delete existing `HealerStats.asset`
3. Right-click > **Create > Gear > Character Stats**
4. Name it `HealerStats`
5. Set these values:
   - Max Health: `120`
   - Move Speed: `5.5`
   - Rotation Speed: `720.0`
   - Attack Damage: `8`
   - Attack Range: `2.5`
   - Attack Cooldown: `1.0`
   - Healing Power: `25`
   - Damage Reduction: `0.0`
   - Character Name: `Healer`
   - Role: `Healer`

### Create Enemy Stats:
1. In `Assets/Data/Enemies/`
2. Delete existing `BasicEnemyStats.asset`
3. Right-click > **Create > Gear > Character Stats**
4. Name it `BasicEnemyStats`
5. Set these values:
   - Max Health: `50`
   - Move Speed: `3.0`
   - Rotation Speed: `360.0`
   - Attack Damage: `10`
   - Attack Range: `2.0`
   - Attack Cooldown: `1.5`
   - Healing Power: `0`
   - Damage Reduction: `0.0`
   - Character Name: `Basic Enemy`
   - Role: `DPS`

## Step 3: Configure Layers

1. Go to **Edit > Project Settings > Tags and Layers**
2. Add these layers:
   - **Layer 6**: `Player`
   - **Layer 7**: `Enemy`
   - **Layer 8**: `Ground`

## Step 4: Create Player Prefab

1. In Hierarchy, right-click > **Create Empty**
2. Name it `Player`
3. Add a visual (right-click Player > **3D Object > Capsule**)
4. Select the Player GameObject (parent)
5. Click **Add Component** and add:
   - **Character Controller**
   - **Network Object**
   - **Player Controller** (your script)
   - **Player Movement** (your script)
   - **Player Health** (your script)
   - **Player Combat** (your script)
   - **Player State Machine** (your script)

6. Configure **Character Controller**:
   - Height: `2`
   - Radius: `0.5`
   - Center: `(0, 1, 0)`

7. Configure **Player Controller**:
   - Drag `TankStats` into the **Stats** field

8. Set Layer to `Player` (top of Inspector)

9. Drag Player from Hierarchy to `Assets/Prefabs/Player/` to create prefab

10. Delete Player from Hierarchy

## Step 5: Create Enemy Prefab

1. In Hierarchy, right-click > **Create Empty**
2. Name it `Enemy`
3. Add a visual (right-click Enemy > **3D Object > Capsule**)
4. Change capsule material to red (optional but helpful)
5. Select the Enemy GameObject (parent)
6. Click **Add Component** and add:
   - **Sphere Collider**
   - **Network Object**
   - **Enemy Controller** (your script)
   - **Enemy Health** (your script)
   - **Enemy AI** (your script)

7. Configure scripts:
   - **Enemy Controller**: Drag `BasicEnemyStats` into **Stats** field
   - **Enemy Health**: Drag `BasicEnemyStats` into **Stats** field

8. Set Layer to `Enemy`

9. Drag Enemy from Hierarchy to `Assets/Prefabs/Enemies/` to create prefab

10. Keep one Enemy in the scene at position `(5, 0, 5)`

## Step 6: Setup Scene

1. If no ground exists, create one:
   - Right-click Hierarchy > **3D Object > Plane**
   - Name it `Ground`
   - Scale: `(10, 1, 10)`
   - Layer: `Ground`

2. Create NetworkManager:
   - Right-click Hierarchy > **Create Empty**
   - Name it `NetworkManager`
   - Add Component: **Network Manager**
   - Add Component: **Unity Transport**
   - In **Network Manager** component:
     - Drag the **Player prefab** from `Assets/Prefabs/Player/` into the **Player Prefab** field

## Step 7: Test the Game

1. Press **Play**
2. In Game view, you should see a NetworkManager GUI
3. Click **Start Host**
4. Use **WASD** to move
5. Press **Space** or **Left Mouse Button** to attack
6. Walk near the enemy to see AI behavior

## Troubleshooting

### "Can't find CharacterStats"
- Make sure you recreated the ScriptableObjects through Unity (Step 2)
- Check that the `[CreateAssetMenu]` attribute is in `CharacterStats.cs`

### "NetworkObject not found"
- Install Netcode for GameObjects package (Step 1)

### Scripts show errors
- Check Console window (Window > General > Console)
- Make sure all packages are installed
- Try: Assets > Reimport All

### Player doesn't move
- Make sure you assigned the stats ScriptableObject
- Check that Character Controller is configured
- Verify layer is set to "Player"

## Quick Start Checklist

- [ ] Install Netcode for GameObjects package
- [ ] Create TankStats ScriptableObject
- [ ] Create HealerStats ScriptableObject  
- [ ] Create BasicEnemyStats ScriptableObject
- [ ] Configure layers (Player, Enemy, Ground)
- [ ] Create Player prefab with all components
- [ ] Create Enemy prefab with all components
- [ ] Setup NetworkManager in scene
- [ ] Add ground plane
- [ ] Press Play and test!
