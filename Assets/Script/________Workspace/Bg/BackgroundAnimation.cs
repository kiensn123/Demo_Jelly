using UnityEngine;
using DG.Tweening;

public class BackgroundAnimation : MonoBehaviour
{
    [Header("--- Transforms ---")]
    public Transform nenTransform;
    public Transform datTransform;
    public Transform mayTransform;

    [Header("--- Cấu hình Mây (May) ---")]
    [Tooltip("Khoảng cách di chuyển theo chiều ngang (X)")]
    public float mayMoveX = 0.5f;
    [Tooltip("Thời gian thực hiện 1 lượt (giây)")]
    public float mayDuration = 3f;
    public Ease mayEase = Ease.InOutSine;

    [Header("--- Cấu hình Đất (Dat) ---")]
    [Tooltip("Khoảng cách di chuyển theo chiều dọc (Y)")]
    public float datMoveY = 0.15f;
    public float datDuration = 2.5f;
    public Ease datEase = Ease.InOutSine;

    [Header("--- Cấu hình Nền/Trời (Nen) ---")]
    [Tooltip("Khoảng cách di chuyển theo chiều dọc (Y)")]
    public float nenMoveY = -0.08f;
    public float nenDuration = 4f;
    public Ease nenEase = Ease.InOutSine;

    void Start()
    {
        AnimateBackground();
    }

    void AnimateBackground()
    {
        // 1. Mây
        if (mayTransform != null)
        {
            mayTransform.DOLocalMoveX(mayTransform.localPosition.x + mayMoveX, mayDuration)
                .SetEase(mayEase)
                .SetLoops(-1, LoopType.Yoyo);
        }

        // 2. Đất
        if (datTransform != null)
        {
            datTransform.DOLocalMoveY(datTransform.localPosition.y + datMoveY, datDuration)
                .SetEase(datEase)
                .SetLoops(-1, LoopType.Yoyo);
        }

        // 3. Nền
        if (nenTransform != null)
        {
            nenTransform.DOLocalMoveY(nenTransform.localPosition.y + nenMoveY, nenDuration)
                .SetEase(nenEase)
                .SetLoops(-1, LoopType.Yoyo);
        }
    }

    // Dọn dẹp Tween khi GameObject bị hủy để tránh rò rỉ bộ nhớ
    private void OnDestroy()
    {
        if (mayTransform != null) mayTransform.DOKill();
        if (datTransform != null) datTransform.DOKill();
        if (nenTransform != null) nenTransform.DOKill();
    }
}