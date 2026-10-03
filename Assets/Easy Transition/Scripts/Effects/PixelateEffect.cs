namespace PixeLadder.EasyTransition.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A transition effect that dissolves the screen into blocky, retro pixels.
    /// </summary>
    [CreateAssetMenu(fileName = "NewPixelateEffect", menuName = "Easy Transition/Pixelate Effect")]
    public class PixelateEffect : TransitionEffect
    {
        public enum AnimationDirection { Reveal, Obscure }

        [Header("Shared Pixel Settings")]
        [Tooltip("Higher values create more, smaller pixels.")]
        [SerializeField, Min(1f)] private float pixelDensity = 5f;

        [Tooltip("Softens the edges of the dissolving pixels.")]
        [Range(0f, 1f)]
        [SerializeField] private float smoothness = 0.2f;
        public override float Smoothness { get => smoothness; set => smoothness = value; }

        private static readonly int DensityProperty = Shader.PropertyToID("_Density");
        private static readonly int SmoothnessProperty = Shader.PropertyToID("_Smoothness");
        private static readonly int OffsetProperty = Shader.PropertyToID("_Offset");

        public override void SetEffectProperties(Material materialInstance)
        {
            materialInstance.SetFloat(SmoothnessProperty, smoothness);
            materialInstance.SetFloat(DensityProperty, pixelDensity);
        }

        public override IEnumerator AnimateOut(Image transitionImage)
        {
            // Set a random offset so the pixel pattern looks unique every time it plays
            transitionImage.material.SetFloat(OffsetProperty, Random.Range(0, 10) * 0.2f);
            yield return AnimateCutoff(transitionImage.material, 1f, 0f, duration / 2f);
        }

        public override IEnumerator AnimateIn(Image transitionImage)
        {
            // Set a slightly different offset for the fade-in
            transitionImage.material.SetFloat(OffsetProperty, Random.Range(0, 10) * 0.2f + 0.1f);
            yield return AnimateCutoff(transitionImage.material, 0f, 1f, duration / 2f);
        }
    }
}