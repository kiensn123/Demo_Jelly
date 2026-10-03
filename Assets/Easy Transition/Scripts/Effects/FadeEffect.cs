namespace PixeLadder.EasyTransition.Effects
{
    using System.Collections;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A simple, classic fade-to-color transition effect.
    /// </summary>
    [CreateAssetMenu(fileName = "NewFadeEffect", menuName = "Easy Transition/Fade Effect")]
    public class FadeEffect : TransitionEffect
    {
        public override IEnumerator AnimateOut(Image transitionImage)
        {
            // Fades the material cutoff from 0 (clear) to 1 (solid).
            yield return AnimateCutoff(transitionImage.material, 0f, 1f, duration / 2f);
        }

        public override IEnumerator AnimateIn(Image transitionImage)
        {
            // Fades the material cutoff from 1 (solid) to 0 (clear).
            yield return AnimateCutoff(transitionImage.material, 1f, 0f, duration / 2f);
        }
    }
}