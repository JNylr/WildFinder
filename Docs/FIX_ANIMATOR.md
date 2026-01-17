# Fix: Animator Not Playing AnimatorController

## The Issue
The Animator component on your character doesn't have the PlayerAnimator controller assigned.

## Fix It

1. **Select the character model**:
   - In Player prefab, select **Idle** → **mixamorig:Hips** (or wherever the Animator component is)

2. **Find the Animator component** in Inspector

3. **Assign the controller**:
   - Look for the **Controller** field (it probably says "None (Runtime Animator Controller)")
   - Click the **circle** next to it
   - Select **PlayerAnimator** from the list
   - OR drag `PlayerAnimator` from `Assets/Animations/` into the Controller field

4. **Save and test**:
   - Exit prefab mode
   - Press Play → Start Host
   - Animations should now work!

## What You Should See
- **Controller field** should show "PlayerAnimator"
- **Avatar** should show the character's avatar
- When you play, animations should trigger based on movement/attacks
