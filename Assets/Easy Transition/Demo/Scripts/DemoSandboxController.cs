namespace PixeLadder.EasyTransition.Demo
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.SceneManagement;
    using TMPro;
    using System.Collections;

    /// <summary>
    /// Core manager for the interactive demo scene.
    /// Handles UI state persistence, day/night toggling, and triggering the transitions.
    /// </summary>
    public class DemoSandboxController : MonoBehaviour
    {
        public static DemoSandboxController Instance { get; private set; }

        [Header("Global Settings UI")]
        [SerializeField] private Toggle loadSceneToggle;
        [SerializeField] private Slider smoothnessSlider;
        [SerializeField] private TMP_Text smoothnessText;
        [SerializeField] private Slider durationSlider;
        [SerializeField] private TMP_Text durationText;
        [SerializeField] private Slider delaySlider;
        [SerializeField] private TMP_Text delayText;
        [SerializeField] private Toggle bgSfxToggle;
        [SerializeField] private Toggle whooshSfxToggle;

        [Header("Background Setup")]
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Image backgroundImageBlurred;
        [SerializeField] private Sprite daySprite;
        [SerializeField] private Sprite daySpriteBlurred;
        [SerializeField] private Sprite nightSprite;
        [SerializeField] private Sprite nightSpriteBlurred;

        [Header("Character Setup (Teleport Showcase)")]
        [SerializeField] private Image characterImage;
        [SerializeField] private Sprite charDayLeft;
        [SerializeField] private Sprite charDayRight;
        [SerializeField] private Sprite charNightLeft;
        [SerializeField] private Sprite charNightRight;

        [Space]
        [SerializeField] private RectTransform shimmerRect;
        [SerializeField] private CanvasGroup panelCanvasGroup;

        // Static variables persist values across scene reloads
        private static bool isLeftPosition = true;
        private static bool isDayTime = true;
        private static float savedSmoothness = 0.2f;
        private static float savedDuration = 1.0f;
        private static float savedDelay = 0.2f;
        public static bool BgSfxEnabled = true;
        public static bool WhooshSfxEnabled = true;

        /// <summary>
        /// Returns true if the user intends to simulate a scene load rather than a same-scene transition.
        /// </summary>
        public static bool IsLoadingScene => Instance != null && Instance.loadSceneToggle != null && Instance.loadSceneToggle.isOn;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            // Restore persistent UI values
            if (smoothnessSlider != null) smoothnessSlider.value = savedSmoothness;
            if (durationSlider != null) durationSlider.value = savedDuration;
            if (delaySlider != null) delaySlider.value = savedDelay;
            if (bgSfxToggle != null) bgSfxToggle.isOn = BgSfxEnabled;
            if (whooshSfxToggle != null) whooshSfxToggle.isOn = WhooshSfxEnabled;

            // Hook up UI listeners
            if (smoothnessSlider != null) smoothnessSlider.onValueChanged.AddListener(UpdateUI);
            if (durationSlider != null) durationSlider.onValueChanged.AddListener(UpdateUI);
            if (delaySlider != null) delaySlider.onValueChanged.AddListener(UpdateUI);
            if (bgSfxToggle != null) bgSfxToggle.onValueChanged.AddListener(UpdateBgSfx);
            if (whooshSfxToggle != null) whooshSfxToggle.onValueChanged.AddListener(UpdateWhooshSfx);

            UpdateUI(0);
            ApplyVisualStates();

            if (SceneTransitioner.Instance != null && SceneTransitioner.Instance.IsTransitioning)
            {
                panelCanvasGroup.interactable = false;
                SceneTransitioner.Instance.OnTransitionFinished.AddListener(EnablePanel);
            }

            ForceUpdateAllLayouts();
        }

        private void OnDestroy()
        {
            if (SceneTransitioner.Instance != null)
            {
                SceneTransitioner.Instance.OnTransitionFinished.RemoveListener(EnablePanel);
            }
        }

        private void EnablePanel()
        {
            panelCanvasGroup.interactable = true;
            SceneTransitioner.Instance.OnTransitionFinished.RemoveListener(EnablePanel);
        }

        private void UpdateUI(float _)
        {
            savedSmoothness = smoothnessSlider != null ? smoothnessSlider.value : savedSmoothness;
            savedDuration = durationSlider != null ? durationSlider.value : savedDuration;
            savedDelay = delaySlider != null ? delaySlider.value : savedDelay;

            if (smoothnessText != null) smoothnessText.text = $"Smoothness\n<align=\"right\">{savedSmoothness:F2}";
            if (durationText != null) durationText.text = $"Duration\n<align=\"right\">{savedDuration:F1}s";
            if (delayText != null) delayText.text = $"Middle Delay\n<align=\"right\">{savedDelay:F1}s";
        }

        private void UpdateBgSfx(bool isEnabled)
        {
            BgSfxEnabled = isEnabled;
            if (DemoAudioManager.Instance != null)
            {
                DemoAudioManager.Instance.UpdateMuteState();
            }
        }

        /// <summary>
        /// Triggered by the individual effect buttons in the demo.
        /// </summary>
        public void PlayEffect(TransitionEffect originalEffect)
        {
            if (SceneTransitioner.Instance == null || SceneTransitioner.Instance.IsTransitioning) return;

            TransitionEffect clonedEffect = Instantiate(originalEffect);
            clonedEffect.name = originalEffect.name + "_Clone";

            clonedEffect.Smoothness = savedSmoothness;
            clonedEffect.Duration = savedDuration;
            clonedEffect.MiddleDelay = savedDelay;

            if (loadSceneToggle != null && loadSceneToggle.isOn)
            {
                isDayTime = !isDayTime;
                SceneTransitioner.Instance.LoadScene(SceneManager.GetActiveScene().name, clonedEffect);
            }
            else
            {
                SceneTransitioner.Instance.PlayTransition(TogglePositionMidpoint, clonedEffect);
            }
        }

        private void ForceUpdateAllLayouts()
        {
            LayoutGroup[] layoutGroups = FindObjectsByType<LayoutGroup>(FindObjectsSortMode.None);
            foreach (LayoutGroup layout in layoutGroups)
            {
                RectTransform rectTransform = layout.GetComponent<RectTransform>();
                if (rectTransform != null) LayoutRebuilder.ForceRebuildLayoutImmediate(rectTransform);
            }
            Canvas.ForceUpdateCanvases();
        }

        public void PlayShimmerAnimation()
        {
            if (shimmerRect != null)
                StartCoroutine(ShimmerRoutine());
        }

        private IEnumerator ShimmerRoutine()
        {
            shimmerRect.gameObject.SetActive(true);

            float duration = 0.4f;
            float time = 0;
            float startX = -630f;
            float endX = 630f;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                float t = time / duration;
                shimmerRect.anchoredPosition = new Vector2(Mathf.Lerp(startX, endX, t), shimmerRect.anchoredPosition.y);
                yield return null;
            }

            shimmerRect.gameObject.SetActive(false);
        }

        private void TogglePositionMidpoint()
        {
            isLeftPosition = !isLeftPosition;
            ApplyVisualStates();
        }

        private void ApplyVisualStates()
        {
            if (backgroundImage != null)
                backgroundImage.sprite = isDayTime ? daySprite : nightSprite;

            if (backgroundImageBlurred != null)
                backgroundImageBlurred.sprite = isDayTime ? daySpriteBlurred : nightSpriteBlurred;

            if (characterImage != null)
            {
                if (isDayTime)
                    characterImage.sprite = isLeftPosition ? charDayLeft : charDayRight;
                else
                    characterImage.sprite = isLeftPosition ? charNightLeft : charNightRight;
            }

            DemoAudioManager.Instance.PlayCorrectTrack(isDayTime);
        }

        private void UpdateWhooshSfx(bool isEnabled)
        {
            WhooshSfxEnabled = isEnabled;
            if (SceneTransitioner.Instance != null && SceneTransitioner.Instance.TryGetComponent(out AudioSource source))
            {
                source.mute = !isEnabled;
            }
        }

        public static float GetCurrentDuration() => savedDuration;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Instance = null;
            isLeftPosition = true;
            isDayTime = true;
            savedSmoothness = 0.2f;
            savedDuration = 1.0f;
            savedDelay = 0.2f;
            BgSfxEnabled = true;
            WhooshSfxEnabled = true;
        }
    }
}