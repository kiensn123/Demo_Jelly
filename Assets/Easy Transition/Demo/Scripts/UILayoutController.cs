namespace PixeLadder.EasyTransition.Demo
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A utility script for the demo scene that globally enables or disables all layout components.
    /// Useful for optimizing UI performance after dynamic layouts have finished building.
    /// </summary>
    public class UILayoutController : MonoBehaviour
    {
        public void SetLayoutState(bool state)
        {
            // Find and toggle all Layout Groups
            LayoutGroup[] layoutGroups = FindObjectsByType<LayoutGroup>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (LayoutGroup layout in layoutGroups)
            {
                layout.enabled = state;
            }

            // Find and toggle all Content Size Fitters
            ContentSizeFitter[] fitters = FindObjectsByType<ContentSizeFitter>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (ContentSizeFitter fitter in fitters)
            {
                fitter.enabled = state;
            }

            // Force a canvas refresh if layouts are being re-enabled
            if (state) Canvas.ForceUpdateCanvases();
        }
    }
}