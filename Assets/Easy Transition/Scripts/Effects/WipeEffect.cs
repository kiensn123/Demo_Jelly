namespace PixeLadder.EasyTransition.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A directional wipe transition effect with customizable entry and exit angles.
    /// </summary>
    [CreateAssetMenu(fileName = "NewWipeEffect", menuName = "Easy Transition/Wipe Effect")]
    public class WipeEffect : TransitionEffect
    {
        public enum WipeDirection
        {
            ToRight, ToLeft, ToTop, ToBottom,
            ToTopRight, ToTopLeft, ToBottomRight, ToBottomLeft
        }

        [Header("Wipe Properties")]
        [Tooltip("The direction the wipe travels during the fade-out phase.")]
        [SerializeField] private WipeDirection fadeOutDirection = WipeDirection.ToRight;

        [Tooltip("The direction the wipe travels during the fade-in phase.")]
        [SerializeField] private WipeDirection fadeInDirection = WipeDirection.ToRight;

        [Tooltip("The softness of the wipe's leading edge.")]
        [Range(0.001f, 1f)]
        [SerializeField] private float smoothness = 0.2f;
        public override float Smoothness { get => smoothness; set => smoothness = value; }

        private static readonly int AngleProperty = Shader.PropertyToID("_Angle");
        private static readonly int SmoothnessProperty = Shader.PropertyToID("_Smoothness");

        public override void SetEffectProperties(Material materialInstance)
        {
            materialInstance.SetFloat(SmoothnessProperty, smoothness);
        }

        public override IEnumerator AnimateOut(Image transitionImage)
        {
            transitionImage.material.SetFloat(AngleProperty, GetAngleForDirection(fadeOutDirection));
            yield return AnimateCutoff(transitionImage.material, 0f, 1f, duration / 2f);
        }

        public override IEnumerator AnimateIn(Image transitionImage)
        {
            // Invert the direction by 180 degrees (Mathf.PI) in shader space for the fade-in
            float angle = GetAngleForDirection(fadeInDirection) + Mathf.PI;
            transitionImage.material.SetFloat(AngleProperty, angle);

            yield return AnimateCutoff(transitionImage.material, 1f, 0f, duration / 2f);
        }

        /// <summary>
        /// Converts the selected WipeDirection enum into radians for the shader math.
        /// </summary>
        private float GetAngleForDirection(WipeDirection dir)
        {
            return dir switch
            {
                WipeDirection.ToTop => 0f * Mathf.Deg2Rad,
                WipeDirection.ToLeft => 90f * Mathf.Deg2Rad,
                WipeDirection.ToBottom => 180f * Mathf.Deg2Rad,
                WipeDirection.ToRight => 270f * Mathf.Deg2Rad,
                WipeDirection.ToTopLeft => 45f * Mathf.Deg2Rad,
                WipeDirection.ToBottomLeft => 135f * Mathf.Deg2Rad,
                WipeDirection.ToBottomRight => 225f * Mathf.Deg2Rad,
                WipeDirection.ToTopRight => 315f * Mathf.Deg2Rad,
                _ => 0f,
            };
        }
    }
}