# Asset Import Guide

## Quick Setup

### 1. Hide Enemy (Do This Now)
1. In Unity Hierarchy, select **Enemy** GameObject
2. At the top of Inspector, **uncheck the checkbox** next to the name
3. Enemy is now hidden but not deleted

### 2. Create Folders (Already Done ✓)
- `Assets/Models/Characters/` - for your character model
- `Assets/Animations/` - for animation files

### 3. Import from Mixamo (Recommended)

**Get Character:**
1. Go to https://www.mixamo.com
2. Sign in (free Adobe account)
3. Click "Characters" tab
4. Choose a character (I recommend "Y Bot" - simple and clean)
5. Click "Download"
   - Format: FBX for Unity
   - Pose: T-Pose
   - Download

**Get Animations:**
1. Click "Animations" tab
2. Search and download these:
   - **"Idle"** - standing still
   - **"Walking"** or **"Running"** - movement
   - **"Sword Slash"** or **"Punching"** - attack
3. For each animation:
   - Select animation
   - Click "Download"
   - Format: FBX for Unity
   - Skin: Without Skin (we only need animation)
   - Download

### 4. Import into Unity

1. **Drag character .fbx** into `Assets/Models/Characters/`
2. **Drag animation .fbx files** into `Assets/Animations/`
3. **Select the character model** in Project window
4. In Inspector, go to **Rig** tab
5. Change **Animation Type** to **Humanoid**
6. Click **Apply**

### 5. Tell Me When Done!

Once you've imported the files, let me know and I'll help you:
- Replace the player capsule with your model
- Set up the Animator Controller
- Add smooth movement
- Add attack animations

---

## Alternative: Use Unity Asset Store

If you prefer Unity Asset Store:
1. Window → Asset Store
2. Search "free character"
3. Download a character pack
4. Import into project
5. Follow same rig setup (Humanoid)
