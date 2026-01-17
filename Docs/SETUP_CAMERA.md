# Setup Camera Follow

## Quick Setup

1. **Select Main Camera** in the Hierarchy

2. **Add the CameraFollow script**:
   - Click "Add Component"
   - Search for "Camera Follow"
   - Add it

3. **Configure the settings** (optional - defaults should work):
   - Offset: `X: 0, Y: 8, Z: -12` (behind and above player)
   - Smooth Speed: `5`
   - Look At Height: `1`

4. **Tag the Player prefab**:
   - Open the Player prefab (`Assets/Prefabs/Player/Player`)
   - At the top of Inspector, change **Tag** from "Untagged" to **"Player"**
   - If "Player" tag doesn't exist, click Tag dropdown → "Add Tag..." → create "Player" tag
   - Save the prefab (Cmd+S)

5. **Test**:
   - Press Play
   - Click Start Host
   - Camera should now follow your player smoothly!

## How It Works
The camera automatically finds the player that belongs to you (IsOwner = true) and follows it with smooth movement. It looks slightly down at the player for a good 3rd person view.
