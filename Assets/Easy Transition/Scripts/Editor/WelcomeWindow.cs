namespace PixeLadder.EasyTransition.Editor
{
    using UnityEngine;
    using UnityEditor;

    /// <summary>
    /// An Editor Window that automatically launches upon first import.
    /// Provides users with instant access to documentation, the demo scene, and support links.
    /// </summary>
    [InitializeOnLoad]
    public class WelcomeWindow : EditorWindow
    {
        private const string PrefKey = "PixeLadder_EasyTransition_ShowWelcome";
        private static bool showOnStartup;

        // Static constructor runs automatically when Unity compiles/reloads
        static WelcomeWindow()
        {
            EditorApplication.delayCall += ShowWindowOnFirstLoad;
        }

        private static void ShowWindowOnFirstLoad()
        {
            // Use SessionState to ensure the window only pops up once per editor session
            if (SessionState.GetBool("EasyTransition_Session_Checked", false)) return;
            SessionState.SetBool("EasyTransition_Session_Checked", true);

            showOnStartup = EditorPrefs.GetBool(PrefKey, true);
            if (showOnStartup)
            {
                ShowWindow();
            }
        }

        [MenuItem("Window/PixeLadder/Easy Transition")]
        public static void ShowWindow()
        {
            WelcomeWindow window = GetWindow<WelcomeWindow>(true, "Easy Transition - Welcome", true);
            window.minSize = new Vector2(350, 400);
            window.maxSize = new Vector2(350, 400);
            window.ShowUtility();
        }

        private void OnEnable()
        {
            showOnStartup = EditorPrefs.GetBool(PrefKey, true);
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 18,
                alignment = TextAnchor.MiddleCenter
            };
            GUILayout.Label("Easy Transition", headerStyle);

            GUIStyle subHeaderStyle = new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleCenter,
                wordWrap = true
            };
            GUILayout.Label("Thank you for downloading.\nCreate seamless scene transitions with zero code.", subHeaderStyle);

            GUILayout.Space(20);

            if (GUILayout.Button("📖 Read the Documentation", GUILayout.Height(30)))
            {
                Application.OpenURL("https://pixeladder-assets.netlify.app/transition_docs");
            }

            if (GUILayout.Button("🎮 Play the Demo Scene", GUILayout.Height(30)))
            {
                string demoScenePath = "Assets/Easy Transition/Demo/Scenes/EasyTransition_DemoScene.unity";

                if (System.IO.File.Exists(demoScenePath))
                {
                    // Prompt user to save their current work before switching scenes
                    if (UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                    {
                        UnityEditor.SceneManagement.EditorSceneManager.OpenScene(demoScenePath);
                    }
                }
                else
                {
                    Debug.LogWarning($"[Easy Transition] Demo scene not found at: {demoScenePath}. Did you move or rename the folder?");
                }
            }

            GUILayout.Space(20);

            EditorGUILayout.HelpBox("If this asset saves you time, leaving a 5-star review is the best way to support its continued development.", MessageType.Info);

            GUIStyle rateStyle = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
            if (GUILayout.Button("Rate Easy Transition\n⭐⭐⭐⭐⭐", rateStyle, GUILayout.Height(45)))
            {
                Application.OpenURL("https://assetstore.unity.com/packages/tools/gui/easy-transition-329334#reviews");
            }

            if (GUILayout.Button("✨ Discover Our Premium Unity Assets", rateStyle, GUILayout.Height(35)))
            {
                Application.OpenURL("https://assetstore.unity.com/publishers/23078");
            }

            GUILayout.FlexibleSpace();

            // Toggle for users to prevent the window from appearing on future startups
            EditorGUI.BeginChangeCheck();
            showOnStartup = GUILayout.Toggle(showOnStartup, " Show this window on startup");
            if (EditorGUI.EndChangeCheck())
            {
                EditorPrefs.SetBool(PrefKey, showOnStartup);
            }

            GUILayout.Space(10);
        }
    }
}