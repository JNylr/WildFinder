# Fix Player Prefab - Assign Stats

## Problem
Player movement crashes with null reference because `stats` is not assigned in the Player prefab.

## Solution

1. **Stop the game** (if running)

2. **Open the Player prefab**:
   - In Project window, navigate to `Assets/Prefabs/Player/`
   - Double-click the **Player** prefab to open it

3. **Assign the stats**:
   - In the Inspector, find the **Player Controller (Script)** component
   - You'll see a **Stats** field that says "None (Character Stats)"
   - Drag **TankStats** from `Assets/Data/Characters/` into the **Stats** field

4. **Save the prefab**:
   - Press `Cmd + S` or File > Save

5. **Test again**:
   - Press Play
   - Click Start Host
   - Try moving with WASD - should work without errors now!
   - Try attacking with Space - should see attack messages

## What This Does
The PlayerController needs to know which stats to use (Tank, Healer, etc.). By assigning TankStats, the player will have the Tank's movement speed, attack damage, and health.
