# Setup Animation Controller

## What We Just Created

I've created `PlayerAnimationController.cs` that will automatically:
- Play **Walk** animation when you move (WASD)
- Play **Idle** animation when you stop
- Play **Attack** animation when you press Space/Left Mouse

## Next Steps in Unity

1. **Add the script to your character**:
   - Open Player prefab (double-click in Project window)
   - Select **Idle** (the character model) in Hierarchy
   - Click **Add Component**
   - Search for **Player Animation Controller**
   - Add it

2. **Test it**:
   - Press Play
   - Click Start Host
   - **Move with WASD** → should play Walk animation
   - **Stop moving** → should play Idle animation
   - **Press Space** → should play Attack animation

3. **If animations are too fast/slow**:
   - Select the animation state in Animator window
   - Adjust **Speed** in Inspector (1 = normal, 0.5 = half speed, 2 = double speed)

## Troubleshooting

**If animations don't play:**
- Make sure PlayerAnimationController is on the **Idle** GameObject (the character model)
- Make sure the Animator Controller is assigned to the Animator component
- Check that all animation states have their Motion set

**If character slides without animating:**
- The animations are playing but might be too subtle
- Try adjusting the Speed parameter

Let me know when you've added the component and tested it!
