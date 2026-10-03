namespace PixeLadder.EasyTransition.Demo
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// A helper script for the demo that forces a UI Image to scale uniformly 
    /// until it completely covers its parent Canvas (similar to CSS background-size: cover).
    /// </summary>
    [RequireComponent(typeof(Image))]
    [ExecuteInEditMode]
    public class FullScreenImageCover : MonoBehaviour
    {
        private RectTransform rect;
        private RectTransform rootCanvasRect;
        private float imgRatio;
        private Vector2 lastSize;

        private void Start()
        {
            Image img = GetComponent<Image>();
            Canvas canvas = GetComponentInParent<Canvas>();

            if (img.sprite == null || canvas == null)
            {
                enabled = false;
                return;
            }

            rect = GetComponent<RectTransform>();
            rootCanvasRect = canvas.rootCanvas.GetComponent<RectTransform>();
            imgRatio = img.sprite.rect.width / img.sprite.rect.height;

            // Center the anchor and pivot
            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);

            AdjustSize();
        }

        private void Update()
        {
            // Only recalculate if the screen/canvas size has actually changed
            if (rootCanvasRect.rect.width != lastSize.x || rootCanvasRect.rect.height != lastSize.y)
            {
                AdjustSize();
            }
        }

        private void AdjustSize()
        {
            float screenW = rootCanvasRect.rect.width;
            float screenH = rootCanvasRect.rect.height;
            lastSize = new Vector2(screenW, screenH);

            // Scale based on the aspect ratio difference
            if ((screenW / screenH) >= imgRatio)
            {
                rect.sizeDelta = new Vector2(screenW, screenW / imgRatio);
            }
            else
            {
                rect.sizeDelta = new Vector2(screenH * imgRatio, screenH);
            }

            transform.position = rootCanvasRect.position;
        }
    }
}