namespace PixeLadder.EasyTransition.Editor
{
    using UnityEngine;
    using UnityEditor;
    using PixeLadder.EasyTransition.Demo;

    /// <summary>
    /// Custom Inspector for the UILayoutController.
    /// Provides simple Editor buttons to quickly enable or disable heavy UI Layout components across the scene.
    /// </summary>
    [CustomEditor(typeof(UILayoutController))]
    public class UILayoutControllerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            UILayoutController controller = (UILayoutController)target;

            GUILayout.Space(10);

            if (GUILayout.Button("Enable All Layouts", GUILayout.Height(30)))
            {
                controller.SetLayoutState(true);
                Debug.Log("[Easy Transition] All Layout Groups and Content Size Fitters ENABLED.");
            }

            GUILayout.Space(5);

            if (GUILayout.Button("Disable All Layouts", GUILayout.Height(30)))
            {
                controller.SetLayoutState(false);
                Debug.Log("[Easy Transition] All Layout Groups and Content Size Fitters DISABLED.");
            }
        }
    }
}