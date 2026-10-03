namespace PixeLadder.EasyTransition
{
    using UnityEngine;
    using UnityEngine.Events;

    /// <summary>
    /// A helper component that acts as a safe bridge between local scene objects and the global SceneTransitioner.
    /// Prevents "Missing Reference" errors when dragging local UI or AudioSources into the global DontDestroyOnLoad prefab.
    /// </summary>
    public class TransitionEventListener : MonoBehaviour
    {
        [Header("Local Scene Events")]
        public UnityEvent OnTransitionStart;
        public UnityEvent OnFadeOutComplete;
        public UnityEvent OnSceneLoaded;
        public UnityEvent OnFadeInStart;
        public UnityEvent OnTransitionFinished;

        private void Start()
        {
            if (SceneTransitioner.Instance != null)
            {
                SceneTransitioner.Instance.OnTransitionStart.AddListener(TriggerStart);
                SceneTransitioner.Instance.OnFadeOutComplete.AddListener(TriggerFadeOut);
                SceneTransitioner.Instance.OnSceneLoaded.AddListener(TriggerLoaded);
                SceneTransitioner.Instance.OnFadeInStart.AddListener(TriggerFadeInStart);
                SceneTransitioner.Instance.OnTransitionFinished.AddListener(TriggerFinished);
            }
        }

        private void OnDestroy()
        {
            if (SceneTransitioner.Instance != null)
            {
                SceneTransitioner.Instance.OnTransitionStart.RemoveListener(TriggerStart);
                SceneTransitioner.Instance.OnFadeOutComplete.RemoveListener(TriggerFadeOut);
                SceneTransitioner.Instance.OnSceneLoaded.RemoveListener(TriggerLoaded);
                SceneTransitioner.Instance.OnFadeInStart.RemoveListener(TriggerFadeInStart);
                SceneTransitioner.Instance.OnTransitionFinished.RemoveListener(TriggerFinished);
            }
        }

        private void TriggerStart() => OnTransitionStart?.Invoke();
        private void TriggerFadeOut() => OnFadeOutComplete?.Invoke();
        private void TriggerLoaded() => OnSceneLoaded?.Invoke();
        private void TriggerFadeInStart() => OnFadeInStart?.Invoke();
        private void TriggerFinished() => OnTransitionFinished?.Invoke();

        /// <summary>
        /// A helper method to safely play a one-shot audio clip globally via the SceneTransitioner.
        /// </summary>
        public void PlayGlobalSound(AudioClip clip)
        {
            if (SceneTransitioner.Instance != null)
            {
                SceneTransitioner.Instance.PlayGlobalSound(clip);
            }
        }
    }
}