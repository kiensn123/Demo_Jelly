namespace PixeLadder.EasyTransition.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A transition effect that uses a Voronoi noise pattern to obscure or reveal the scene.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCellularEffect", menuName = "Easy Transition/Cellular Effect")]
    public class CellularEffect : TransitionEffect
    {
        public enum AnimationDirection { Upsize, Downsize }

        [Header("Fade Out Settings")]
        [Tooltip("The direction of the cellular growth during the fade-out phase.")]
        [SerializeField] private AnimationDirection fadeOutAnimation = AnimationDirection.Downsize;

        [Header("Fade In Settings")]
        [Tooltip("The direction of the cellular growth during the fade-in phase.")]
        [SerializeField] private AnimationDirection fadeInAnimation = AnimationDirection.Upsize;

        [Header("Shared Settings")]
        [Tooltip("The density and scale of the cellular pattern.")]
        [SerializeField, Min(1f)] private float density = 10.0f;

        [Tooltip("The speed at which the cells organically shift and evolve over time.")]
        [SerializeField, Min(0f)] private float cellSpeed = 15.0f;

        [Tooltip("The softness of the cell edges.")]
        [Range(0.001f, 1f)]
        [SerializeField] private float smoothness = 0.1f;
        public override float Smoothness { get => smoothness; set => smoothness = value; }

        // Cached shader property IDs for performance
        private static readonly int InvertProperty = Shader.PropertyToID("_Invert");
        private static readonly int DensityProperty = Shader.PropertyToID("_Density");
        private static readonly int SpeedProperty = Shader.PropertyToID("_CellSpeed");
        private static readonly int SmoothnessProperty = Shader.PropertyToID("_Smoothness");

        public override void SetEffectProperties(Material materialInstance)
        {
            materialInstance.SetFloat(SmoothnessProperty, smoothness);
            materialInstance.SetFloat(SpeedProperty, cellSpeed);
            materialInstance.SetFloat(DensityProperty, density);
        }

        public override IEnumerator AnimateOut(Image transitionImage)
        {
            transitionImage.material.SetFloat(InvertProperty, fadeOutAnimation == AnimationDirection.Upsize ? 1.0f : 0.0f);

            float startValue = (fadeOutAnimation == AnimationDirection.Upsize) ? 0f : 1f;
            float endValue = (fadeOutAnimation == AnimationDirection.Upsize) ? 1f : 0f;

            yield return AnimateCutoff(transitionImage.material, startValue, endValue, duration / 2f);
        }

        public override IEnumerator AnimateIn(Image transitionImage)
        {
            transitionImage.material.SetFloat(InvertProperty, fadeInAnimation == AnimationDirection.Downsize ? 1.0f : 0.0f);

            float startValue = (fadeInAnimation == AnimationDirection.Upsize) ? 0f : 1f;
            float endValue = (fadeInAnimation == AnimationDirection.Upsize) ? 1f : 0f;

            yield return AnimateCutoff(transitionImage.material, startValue, endValue, duration / 2f);
        }
    }
}