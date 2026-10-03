# Easy Transition v1.1.2 by Ahmed Benlakhdhar

A flexible, ScriptableObject-based scene transition system for Unity.


## Quick Start

1.  **Add the Manager:** Drag the `SceneTransitioner` prefab from `Easy Transition/Prefabs/` into your first scene.

2.  **Create an Effect:** In the Project window, right-click `Create -> Easy Transition` to add a new effect like `Fade` or `Wipe`.

3.  **Call from Script:**
    ```csharp
    // Load a new scene by Name
    SceneTransitioner.Instance.LoadScene("YourSceneName", yourTransitionEffect);

    // Or load a new scene by Build Index
    SceneTransitioner.Instance.LoadScene(1, yourTransitionEffect);

    // Or trigger a transition without loading a scene (e.g., teleporting)
    SceneTransitioner.Instance.PlayTransition(YourTeleportMethod, yourTransitionEffect);
    ```


## Key Features

- **Asynchronous Loading** (Prevents Game Freezes)
- **ScriptableObject-Based Architecture**
- **TimeScale Independent** (Transitions work perfectly even when the game is paused)
- **Same-Scene Transitions** (Execute mid-point actions without changing scenes)
- **VR Ready** (Supports ScreenSpaceCamera & WorldSpace render modes. Tested on Meta Quest 2)
- **Optional UI Blocking** (Safely blocks player clicks during transitions to prevent bugs)
- **Easily Extensible** to Create Custom Effects
- **Full C# Source Code Included**
- **6 Effect Types Included** (Fade, Wipe, Circle, Cellular, Smoke, and Pixelate)
- **Interactive Sandbox Demo Scene Provided**
- **Custom Animation Easing** (Use built-in Unity Animation Curves to apply ultra-smooth pacing)
- **Safe Event Hooks** (Easily sync local audio and particles using the included TransitionEventListener)


## Support

For the full manual, see the Documentation folder.

⭐⭐⭐⭐⭐ **Leave a Rating**

If Easy Transition saves you time and helps your project, please consider leaving a 5-star review on the [Asset Store page](https://assetstore.unity.com/packages/tools/gui/easy-transition-329334#reviews). It helps the asset grow immensely.

**Need Support?**  
Email is the fastest way to reach me. If you encounter any bugs, need help, or have feature requests, please contact me directly *before* leaving a review so I can resolve it for you immediately:
*   **Email:** [pixeladderdev@gmail.com](mailto:pixeladderdev@gmail.com)  
*(Please include "[Easy Transition]" in the email subject line so it doesn't get caught in spam.)*