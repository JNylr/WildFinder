# Troubleshooting: "Gear" Not Showing in Create Menu

## Quick Fixes to Try (in order):

### 1. Force Unity to Refresh
In Unity Editor:
- Go to **Assets > Refresh** (or press `Cmd+R` on Mac)
- Wait for Unity to finish processing

### 2. Reimport the CharacterStats Script
1. In Project window, navigate to `Assets/Scripts/Data/`
2. Right-click on `CharacterStats.cs`
3. Select **Reimport**
4. Wait for Unity to finish

### 3. Check Console for Errors
1. Open **Window > General > Console**
2. Look for any **red error messages**
3. If you see errors, send me a screenshot

### 4. Verify Script Compilation
Look at the bottom-right corner of Unity:
- It should say something like "All scripts compiled" or show no spinning icon
- If it's still compiling, wait for it to finish

### 5. Try Reimport All (Nuclear Option)
If nothing else works:
1. Go to **Assets > Reimport All**
2. This will take a few minutes
3. Wait for Unity to finish completely

### 6. Check if Unity Sees the Script
Try this test:
1. Create an empty GameObject in the scene
2. Try to add `CharacterStats` as a component
3. If you can't find it, there's a compilation issue

## Alternative: Manual Asset Creation

If the Create menu still doesn't work, we can create the assets a different way:

1. In Project window, go to `Assets/Data/Characters/`
2. Right-click > **Create > ScriptableObject**
3. In the dialog, type: `Gear.Data.CharacterStats`
4. Click Create
5. Name it `TankStats`

Let me know which step reveals the issue!
