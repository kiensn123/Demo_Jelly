namespace PixeLadder.EasyTransition
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// The base class for all custom transition effects.
    /// Inherit from this to create your own ScriptableObject-based transitions.
    /// </summary>
    public abstract class TransitionEffect : ScriptableObject
    {
        [Tooltip("The shader material used for this transition effect.")]
        public Material transitionMaterial;

        [Header("Timing")]
        [Tooltip("Delay in seconds between the fade-out and the fade-in.")]
        [SerializeField, Min(0)] protected float middleDelay = 0f;

        [Tooltip("Total duration of the transition effect in seconds.")]
        [SerializeField, Min(0.1f)] protected float duration = 1.0f;

        public virtual float Smoothness { get; set; }
        public float MiddleDelay { get => middleDelay; set => middleDelay = Mathf.Max(0f, value); }
        public float Duration { get => duration; set => duration = Mathf.Max(0.1f, value); }

        [Header("Animation")]
        [Tooltip("Controls the easing and pacing of the transition progression. Default is a linear curve.")]
        [SerializeField] protected AnimationCurve easingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        /// <summary>
        /// Defines the logic and progression of the screen being obscured.
        /// </summary>
        public abstract IEnumerator AnimateOut(Image transitionImage);

        /// <summary>
        /// Defines the logic and progression of the screen being revealed.
        /// </summary>
        public abstract IEnumerator AnimateIn(Image transitionImage);

        /// <summary>
        /// Override this to pass custom properties to the instantiated material before the animation begins.
        /// </summary>
        public virtual void SetEffectProperties(Material materialInstance) { }

        /// <summary>
        /// Animates the material's _Cutoff property over time using the defined easing curve.
        /// Uses unscaled time to function perfectly even when Time.timeScale is 0.
        /// </summary>
        protected IEnumerator AnimateCutoff(Material materialInstance, float startValue, float targetValue, float animDuration)
        {
            materialInstance.SetFloat("_Cutoff", startValue);
            float time = 0;

            while (time < animDuration)
            {
                float t = Mathf.Clamp01(time / animDuration);

                // Smooth the linear progress using the defined AnimationCurve
                float curvedT = easingCurve.Evaluate(t);

                float cutoffValue = Mathf.Lerp(startValue, targetValue, curvedT);
                materialInstance.SetFloat("_Cutoff", cutoffValue);

                yield return null;

                // Time is capped at 0.1s to prevent massive animation skipping if the Editor lags during scene loads
                time += Mathf.Min(Time.unscaledDeltaTime, 0.1f);
            }

            // Guarantee the transition hits the exact final value upon completion
            float finalCurvedT = easingCurve.Evaluate(1f);
            materialInstance.SetFloat("_Cutoff", Mathf.Lerp(startValue, targetValue, finalCurvedT));
        }
    }
}