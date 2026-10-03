namespace PixeLadder.EasyTransition.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A transition effect that reveals or obscures the screen using an expanding or shrinking circular mask.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCircleEffect", menuName = "Easy Transition/Circle Effect")]
    public class CircleEffect : TransitionEffect
    {
        public enum AnimationDirection { CenterToEdge, EdgeToCenter }

        [Header("Fade Out Settings")]
        [Tooltip("The direction the circle expands or shrinks during the fade-out phase.")]
        [SerializeField] private AnimationDirection fadeOutAnimation = AnimationDirection.CenterToEdge;

        [Header("Fade In Settings")]
        [Tooltip("The direction the circle expands or shrinks during the fade-in phase.")]
        [SerializeField] private AnimationDirection fadeInAnimation = AnimationDirection.EdgeToCenter;

        [Header("Shared Settings")]
        [Tooltip("The softness of the circle's edge. A lower value creates a sharper edge.")]
        [Range(0.001f, 1f)]
        [SerializeField] private float smoothness = 0.1f;
        public override float Smoothness { get => smoothness; set => smoothness = value; }

        // Cached shader property IDs for performance
        private static readonly int InvertProperty = Shader.PropertyToID("_Invert");
        private static readonly int SmoothnessProperty = Shader.PropertyToID("_Smoothness");

        public override void SetEffectProperties(Material materialInstance)
        {
            materialInstance.SetFloat(SmoothnessProperty, smoothness);
        }

        public override IEnumerator AnimateOut(Image transitionImage)
        {
            transitionImage.material.SetFloat(InvertProperty, fadeOutAnimation == AnimationDirection.CenterToEdge ? 1.0f : 0.0f);

            float startValue = fadeOutAnimation == AnimationDirection.CenterToEdge ? 0f : 1f;
            float endValue = fadeOutAnimation == AnimationDirection.CenterToEdge ? 1f : 0f;

            yield return AnimateCutoff(transitionImage.material, startValue, endValue, duration / 2f);
        }

        public override IEnumerator AnimateIn(Image transitionImage)
        {
            transitionImage.material.SetFloat(InvertProperty, fadeInAnimation == AnimationDirection.EdgeToCenter ? 1.0f : 0.0f);

            float startValue = fadeInAnimation == AnimationDirection.CenterToEdge ? 0f : 1f;
            float endValue = fadeInAnimation == AnimationDirection.CenterToEdge ? 1f : 0f;

            yield return AnimateCutoff(transitionImage.material, startValue, endValue, duration / 2f);
        }
    }
}