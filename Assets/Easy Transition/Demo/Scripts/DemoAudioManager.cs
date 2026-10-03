namespace PixeLadder.EasyTransition.Demo
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// A standalone audio manager for the demo scene. 
    /// Hooks into the SceneTransitioner events to smoothly fade background SFX in and out.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class DemoAudioManager : MonoBehaviour
    {
        public static DemoAudioManager Instance;

        [Header("Audio Tracks")]
        [Tooltip("The background SFX track played during the Day state.")]
        [SerializeField] private AudioClip daySound;

        [Tooltip("The background SFX track played during the Night state.")]
        [SerializeField] private AudioClip nightSound;

        [Header("Settings")]
        [Tooltip("The maximum volume the background SFX will reach after fading in.")]
        [SerializeField] private float maxVolume = 0.5f;

        private AudioSource audioSource;
        private Coroutine fadeRoutine;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                audioSource = GetComponent<AudioSource>();
                audioSource.loop = true;
                audioSource.volume = maxVolume;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            if (SceneTransitioner.Instance != null)
            {
                SceneTransitioner.Instance.OnTransitionStart.AddListener(FadeOut);
                SceneTransitioner.Instance.OnFadeInStart.AddListener(FadeIn);
            }

            UpdateMuteState();
            PlayCorrectTrack(true);
        }

        /// <summary>
        /// Syncs the AudioSource mute state with the demo's UI toggle.
        /// </summary>
        public void UpdateMuteState()
        {
            if (audioSource != null)
            {
                audioSource.mute = !DemoSandboxController.BgSfxEnabled;
            }
        }

        /// <summary>
        /// Swaps the background SFX track based on the current demo state.
        /// </summary>
        public void PlayCorrectTrack(bool isDay)
        {
            AudioClip targetClip = isDay ? daySound : nightSound;
            if (audioSource.clip != targetClip)
            {
                audioSource.clip = targetClip;
                audioSource.Play();
            }
        }

        private void FadeOut()
        {
            if (DemoSandboxController.IsLoadingScene)
                TriggerFade(0f, DemoSandboxController.GetCurrentDuration() / 2f);
        }

        private void FadeIn()
        {
            if (DemoSandboxController.IsLoadingScene)
                TriggerFade(maxVolume, DemoSandboxController.GetCurrentDuration() / 2f);
        }

        private void TriggerFade(float targetVol, float duration)
        {
            if (fadeRoutine != null) StopCoroutine(fadeRoutine);
            fadeRoutine = StartCoroutine(FadeRoutine(targetVol, duration));
        }

        private IEnumerator FadeRoutine(float targetVol, float duration)
        {
            float startVol = audioSource.volume;
            float time = 0;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                audioSource.volume = Mathf.Lerp(startVol, targetVol, time / duration);
                yield return null;
            }

            audioSource.volume = targetVol;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            Instance = null;
        }
    }
}