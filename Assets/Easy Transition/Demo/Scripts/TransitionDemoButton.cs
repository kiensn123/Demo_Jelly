namespace PixeLadder.EasyTransition.Demo
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A helper script for the demo scene. 
    /// Attaches to a UI Button to trigger a specific transition effect in the Demo Sandbox.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public class TransitionDemoButton : MonoBehaviour
    {
        [Tooltip("The specific effect to play when this button is clicked.")]
        [SerializeField] private TransitionEffect assignedEffect;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        private void OnClick()
        {
            if (assignedEffect != null && DemoSandboxController.Instance != null)
            {
                DemoSandboxController.Instance.PlayEffect(assignedEffect);
            }
            else
            {
                Debug.LogWarning("[Easy Transition] Missing effect or DemoSandboxController.");
            }
        }
    }
}