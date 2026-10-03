# Easy Transition

### User Manual & Documentation

**Version 1.1.2**  
**Created by Ahmed Benlakhdhar**

---

### **Table of Contents**
1.  Introduction
2.  Quick Start Guide
3.  The ScriptableObject Workflow
4.  Core Components
5.  Configuration
6.  Creating Custom Effects
7.  FAQ & Support

---

### **1. Introduction**

Thank you for choosing Easy Transition! This asset is a powerful and flexible system for creating professional, animated scene transitions in your Unity project, built on a robust ScriptableObject architecture.

**Key Features:**
*   **Asynchronous Loading:** Prevents game freezes and stutters between scenes.
*   **ScriptableObject Based:** Create and configure endless transition variations without writing new code.
*   **TimeScale Independent:** Transitions animate smoothly even if `Time.timeScale` is set to 0.
*   **Same-Scene Transitions:** Run custom logic (like player teleportation) right at the midpoint of a transition.
*   **VR & UI Ready:** Built-in support for multiple Canvas render modes, tested on Meta Quest 2, and features optional automatic UI input blocking.
*   **Easily Extensible:** Create your own custom effects by inheriting from the `TransitionEffect` class.
*   **Full C# Source Code Included**
*   **6 Effect Types Included:** Fade, Wipe, Circle, Cellular, Smoke, and Pixelate.
*   **Custom Animation Easing:** Ditch linear fades by using built-in Unity Animation Curves to apply ultra-smooth pacing (like Ease-In-Out) to your transitions.
*   **Safe Event Hooks:** Easily sync local audio and particles to transition stages without causing missing reference errors on scene load.

---

### **2. Quick Start Guide**

1.  **Add the Manager:**
    Drag the `SceneTransitioner` prefab from `Easy Transition/Prefabs/` into your *first* scene (e.g., your Main Menu).

2.  **Create an Effect:**
    In your Project window, right-click and go to `Create -> Easy Transition` to create a new `Fade Effect`, `Wipe Effect`, etc. Select this new asset and configure its settings in the Inspector.

3.  **Call from Script:**
    From any script, call the transition using the effect you just created. You have three flexible options:

    ```csharp
    // 1. Load by Scene Name
    SceneTransitioner.Instance.LoadScene("YourSceneName", yourTransitionEffect);

    // 2. Load by Build Index
    SceneTransitioner.Instance.LoadScene(1, yourTransitionEffect);

    // 3. Play a transition without changing scenes (pass an Action for the midpoint)
    SceneTransitioner.Instance.PlayTransition(YourCustomMethod, yourTransitionEffect);
    ```

**Done!** See the Demo Scene in `Assets/Easy Transition/Demo/Scenes/` for an interactive live sandbox.

---

### **3. The ScriptableObject Workflow**

The core of this asset is the `TransitionEffect` ScriptableObject. Instead of having one complex manager, you create small, reusable "Effect" assets in your project. This allows you to create many different transition styles and variations easily without coding.

For example, you can create a "FastWipe.asset" and a "SlowWipe.asset" from the same `WipeEffect` script, just with different duration or smoothness values.

---

### **4. Core Components**

*   **`SceneTransitioner` Prefab:** The "brain" of the system. A singleton that persists across scenes and runs the transitions. You only need one in your very first scene.
*   **`TransitionEffect` Scripts:** The ScriptableObject assets that define what a transition looks like (e.g., `FadeEffect`, `CircleEffect`).
*   **`TransitionEventListener` Script:** A helper component used to safely link local scene objects (like UI or AudioSources) to the global transition events without causing missing reference errors when scenes reload.
*   **Unity Events:** The system provides clean event hooks (`OnTransitionStart`, `OnFadeOutComplete`, `OnSceneLoaded`, `OnFadeInStart`, `OnTransitionFinished`) allowing you to easily sync sound, UI, or gameplay logic with the transition lifecycle.

---

### **5. Configuration**

You can configure your transitions globally or on a case-by-case basis:

**1. Default Transitions:**
Set the **"Default Transition"** field on the `SceneTransitioner` prefab. If you call `LoadScene()` without specifying an effect, this one will be used automatically.

**2. Per-Transition Override:**
Pass a specific `TransitionEffect` asset directly into the `LoadScene()` or `PlayTransition()` method. This is the recommended approach.

**3. Canvas Setup (VR & Sorting):**
On the `SceneTransitioner` prefab, you can adjust the **Render Mode**. Standard games should use `ScreenSpaceOverlay`. If you are developing for VR, switch this to `ScreenSpaceCamera` and assign your main camera. You can also adjust the **Canvas Sorting Order** here to ensure transitions always render on top of your UI.

---

### **6. Creating Custom Effects**

You can easily create your own effects:
1. Create a new C# script that inherits from `TransitionEffect`.
2. Implement the `AnimateOut` and `AnimateIn` methods.
3. Add a `[CreateAssetMenu]` attribute to your new class.

You can look at `FadeEffect.cs` or `WipeEffect.cs` for a clear example of how to interact with the material properties.

---

### **7. FAQ & Support**

**Q: My transition is not appearing on top of my UI.**  
A: The `SceneTransitioner` automatically creates its own Canvas with a very high "Sort Order" (999). If you are using multiple Canvases with custom Sort Orders, you can increase the Sort Order setting directly on the `SceneTransitioner` prefab.

**Q: Does this work in VR?**  
A: Yes! On the `SceneTransitioner` prefab, change the Canvas Render Mode from `ScreenSpaceOverlay` to `ScreenSpaceCamera` and assign your player's camera (Tested on Meta Quest 2). The system will automatically handle the rest.

**Q: How do I play a sound or trigger a particle effect during a transition?**  
A: Attach the `TransitionEventListener` component to any object in your scene. You can plug your local AudioSources or ParticleSystems directly into its UnityEvents in the Inspector. It safely connects to the global manager without throwing errors when the scene unloads, and even includes a handy `PlayGlobalSound()` method.

**Q: Will the transition freeze if I pause my game?**  
A: No. The transition system uses unscaled time, meaning it will animate smoothly even if `Time.timeScale` is set to 0 (perfect for pause menus or dramatic slow-motion effects).

**Q: Can players click buttons while the screen is fading?**  
A: By default, no. The `SceneTransitioner` automatically enables a hidden CanvasGroup that safely blocks all raycasts/clicks during the entire transition pipeline. You can disable this behavior by unchecking "Block UI Interaction" on the prefab.

**Q: How can I fade my background music during a transition?**
A: Look at `DemoAudioManager.cs` in the Demo folder for a clean example of how to use the UnityEvents to mathematically fade your music out and in, perfectly synced with the screen transition.

⭐⭐⭐⭐⭐ **Leave a Rating**

If Easy Transition saves you time and helps your project, please consider leaving a 5-star review on the [Asset Store page](https://assetstore.unity.com/packages/tools/gui/easy-transition-329334#reviews). It helps the asset grow immensely.

**Need Support?**  
Email is the fastest way to reach me. If you encounter any bugs, need help, or have feature requests, please contact me directly *before* leaving a review so I can resolve it for you immediately:
*   **Email:** [pixeladderdev@gmail.com](mailto:pixeladderdev@gmail.com)  
*(Please include "[Easy Transition]" in the email subject line so it doesn't get caught in spam.)*