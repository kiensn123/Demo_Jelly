namespace PixeLadder.EasyTransition.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A transition effect that reveals or obscures the screen using a scrolling Perlin noise pattern.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSmokeEffect", menuName = "Easy Transition/Smoke Effect")]
    public class SmokeEffect : TransitionEffect
    {
        public enum AnimationDirection { Reveal, Obscure }

        [Header("Shared Smoke Settings")]
        [Tooltip("The density and scale of the smoke noise.")]
        [SerializeField, Min(0.1f)] private float smokeDensity = 1.0f;

        [Tooltip("The speed at which the smoke scrolls across the screen.")]
        [SerializeField] private float scrollSpeed = 0.3f;

        [Tooltip("The directional angle of the smoke's movement in degrees.")]
        [SerializeField, Range(0f, 360f)] private float scrollAngle = 0f;

        [Tooltip("The softness of the smoke's edge. A lower value creates a sharper, more solid wall of smoke.")]
        [Range(0.001f, 1f)]
        [SerializeField] private float smoothness = 0.2f;
        public override float Smoothness { get => smoothness; set => smoothness = value; }

        private static readonly int InvertProperty = Shader.PropertyToID("_Invert");
        private static readonly int DensityProperty = Shader.PropertyToID("_Density");
        private static readonly int ScrollDirectionProperty = Shader.PropertyToID("_ScrollDirection");
        private static readonly int SpeedProperty = Shader.PropertyToID("_ScrollSpeed");
        private static readonly int SmoothnessProperty = Shader.PropertyToID("_Smoothness");

        public override void SetEffectProperties(Material materialInstance)
        {
            materialInstance.SetFloat(SmoothnessProperty, smoothness);
            materialInstance.SetFloat(DensityProperty, smokeDensity);
            materialInstance.SetFloat(SpeedProperty, scrollSpeed);
            materialInstance.SetFloat(ScrollDirectionProperty, scrollAngle);
        }

        public override IEnumerator AnimateOut(Image transitionImage)
        {
            transitionImage.material.SetFloat(InvertProperty, 0.0f);
            yield return AnimateCutoff(transitionImage.material, 0f, 1f, duration / 2f);
        }

        public override IEnumerator AnimateIn(Image transitionImage)
        {
            transitionImage.material.SetFloat(InvertProperty, 1.0f);
            yield return AnimateCutoff(transitionImage.material, 0f, 1f, duration / 2f);
        }
    }
}