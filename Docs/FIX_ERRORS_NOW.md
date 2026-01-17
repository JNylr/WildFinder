# IMMEDIATE FIX - Unity Console Errors

## The Problem
Unity has cached broken asset files that no longer exist. You're seeing 22 errors about "Could not extract GUID" and "Broken text PPtr".

## The Solution (Do This Now)

### Option 1: Quick Restart (RECOMMENDED - 30 seconds)
1. **Quit Unity completely** (Cmd+Q or File > Quit)
2. **Wait 5 seconds**
3. **Reopen Unity** (it will reload the project)
4. **Wait for compilation to finish** (watch bottom-right corner)
5. **Check Console** - errors should be GONE
6. **Try Create menu** - "Gear" should now appear

### Option 2: Manual Cache Clear (If restart doesn't work)
1. Quit Unity
2. Delete Unity's cache:
   ```bash
   rm -rf /Users/james/Projects/Gear/Library/StateCache
   rm -rf /Users/james/Projects/Gear/Library/SourceAssetDB
   ```
3. Reopen Unity
4. Wait for reimport (will take 1-2 minutes)

## Why This Happens
The `.asset` files I created were text files, but Unity expects binary files. I've deleted them from disk, but Unity still has them cached in memory. A restart clears this cache.

## After Restart
Once Unity restarts with no errors:
1. Navigate to `Assets/Data/Characters/`
2. Right-click > **Create > Gear > Character Stats**
3. You should now see this option!

## Still Have Errors?
If you still see errors after restart, send me a screenshot of the Console window.
