using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HorizontalExpandMenu : MonoBehaviour
{
    [Header("Main Button")]
    [SerializeField] private Button mainButton;
    [SerializeField] private Transform mainIcon; // Transform của Icon nút cha để xoay/đổi hình

    [Header("Sub Menu Container")]
    [SerializeField] private CanvasGroup subMenuCanvasGroup; // CanvasGroup trên Object chứa các nút con
    [SerializeField] private RectTransform subMenuHolder;    // RectTransform chứa danh sách các button tròn

    [Header("Layout Settings")]
    [SerializeField] private float spacing = 90f;        // Khoảng cách giữa tâm các nút con
    [SerializeField] private float startOffsetX = 20f;    // Khoảng cách từ nút cha đến nút con đầu tiên
    [SerializeField] private float animationDuration = 0.35f;

    [Header("DOTween Settings")]
    [SerializeField] private Ease openEase = Ease.OutBack;   // Hiệu ứng nảy nhẹ khi mở
    [SerializeField] private Ease closeEase = Ease.InQuad;

    private List<RectTransform> childButtons = new List<RectTransform>();
    private bool isOpen = false;
    private Sequence menuSequence;

    private void Awake()
    {
        // 1. Lấy danh sách tất cả button con trong subMenuHolder
        foreach (Transform child in subMenuHolder)
        {
            if (child.gameObject.activeSelf)
            {
                childButtons.Add(child as RectTransform);

                // Thêm lắng nghe click cho từng nút con để tự động đóng menu
                Button btn = child.GetComponent<Button>();
                if (btn != null)
                {
                    btn.onClick.AddListener(OnChildButtonClicked);
                }
            }
        }

        // 2. Lắng nghe sự kiện click nút cha
        if (mainButton != null)
        {
            mainButton.onClick.AddListener(ToggleMenu);
        }

        // 3. Khởi tạo trạng thái ban đầu (Đóng)
        InitState();
    }

    private void InitState()
    {
        subMenuCanvasGroup.alpha = 0f;
        subMenuCanvasGroup.blocksRaycasts = false;
        subMenuCanvasGroup.interactable = false;

        // Đưa tất cả nút con về mốc X = 0, scale = 0
        foreach (var btn in childButtons)
        {
            btn.anchoredPosition = Vector2.zero;
            btn.localScale = Vector3.zero;
        }
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;

        // Kill sequence cũ nếu người chơi spam click nhanh
        menuSequence?.Kill();
        menuSequence = DOTween.Sequence();

        if (isOpen)
        {
            OpenMenu();
        }
        else
        {
            CloseMenu();
        }
    }

    private void OpenMenu()
    {
        subMenuCanvasGroup.blocksRaycasts = true;
        subMenuCanvasGroup.interactable = true;

        // Fade In CanvasGroup
        menuSequence.Join(subMenuCanvasGroup.DOFade(1f, animationDuration * 0.5f));

        // Xoay nhẹ Icon nút cha (ví dụ xoay 45 độ tạo cảm giác mở)
        if (mainIcon)
            menuSequence.Join(mainIcon.DORotate(new Vector3(0, 0, 45f), animationDuration));

        // Tính toán vị trí X và bung từng nút con từ trái sang phải
        for (int i = 0; i < childButtons.Count; i++)
        {
            float targetX = startOffsetX + (i * spacing);
            float delay = i * 0.04f; // Delay nhỏ giữa các nút để tạo hiệu ứng gợn sóng (stagger)

            // Di chuyển X
            menuSequence.Insert(delay, childButtons[i].DOAnchorPosX(targetX, animationDuration).SetEase(openEase));
            // Phóng to nút từ 0 lên 1
            menuSequence.Insert(delay, childButtons[i].DOScale(Vector3.one, animationDuration).SetEase(openEase));
        }
    }

    private void CloseMenu()
    {
        subMenuCanvasGroup.blocksRaycasts = false;
        subMenuCanvasGroup.interactable = false;

        // Fade Out CanvasGroup
        menuSequence.Join(subMenuCanvasGroup.DOFade(0f, animationDuration));

        // Xoay Icon nút cha về lại vị trí ban đầu
        if (mainIcon)
            menuSequence.Join(mainIcon.DORotate(Vector3.zero, animationDuration));

        // Thu từng nút con về gốc X = 0 và scale = 0
        for (int i = childButtons.Count - 1; i >= 0; i--)
        {
            float delay = (childButtons.Count - 1 - i) * 0.03f;

            menuSequence.Insert(delay, childButtons[i].DOAnchorPosX(0f, animationDuration).SetEase(closeEase));
            menuSequence.Insert(delay, childButtons[i].DOScale(Vector3.zero, animationDuration).SetEase(closeEase));
        }
    }

    // Tự động thu menu khi bấm bất kỳ nút con nào
    private void OnChildButtonClicked()
    {
        if (isOpen)
        {
            ToggleMenu();
        }
    }

    private void OnDestroy()
    {
        menuSequence?.Kill();
    }
}