// FillZone.cs
using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class FillZone : MonoBehaviour
{
    public Collider2D zone;               // vùng cần lấp (Is Trigger)
    [Range(0.5f, 1f)] public float insideRatio = 1f;
    public float stillTime = 0.5f;        // vào đủ liên tục bao lâu thì thắng
    public bool debugLog = true;

    [Tooltip("Kéo các JellyPiece của level vào đây")]
    public List<JellyPiece> pieces = new();
    float stillTimer;

    int lastInside = -1;
    float nextLog;

    public int InsideCount { get; private set; }

    private bool IsStops;

    [Header("Đường kẻ")]
    public List<SpriteRenderer> guideLines = new();          // kéo các object đường kẻ vào đây
    public Color colorNormal = Color.black;
    public Color colorInside = new Color(0.2f, 0.9f, 0.3f);  // xanh lá
    public float tweenTime = 0.25f;
    bool allInside;

    void Start()
    {
        if (!zone) { Debug.LogError("[FillZone] Chưa gán ô Zone"); enabled = false; return; }

        pieces.RemoveAll(p => p == null);
        if (pieces.Count == 0) { Debug.LogError("[FillZone] Chưa kéo mảnh nào vào list"); enabled = false; return; }

        foreach (var g in guideLines)
            if (g) g.color = colorNormal;

        if (debugLog) Debug.Log($"[FillZone] Sẵn sàng. Zone={zone.name}, mảnh={pieces.Count}, bounds={zone.bounds}");
    }

    // tỉ lệ bone của mảnh nằm trong vùng
    float RatioOf(JellyPiece p)
    {
        if (p.bones.Count == 0) return 0f;
        int inside = 0;
        foreach (var b in p.bones)
            if (zone.OverlapPoint(b.position)) inside++;
        return (float)inside / p.bones.Count;
    }

    void SetGuideColor(bool inside)
    {
        Color target = inside ? colorInside : colorNormal;
        foreach (var g in guideLines)
        {
            if (!g) continue;
            g.DOKill();
            g.DOColor(target, tweenTime);
        }
    }

    void OnDestroy()
    {
        foreach (var g in guideLines)
            if (g) g.DOKill();
    }

    void Update()
    {
        if (IsStops || pieces.Count == 0) return;

        InsideCount = 0;
        string detail = "";

        foreach (var p in pieces)
        {
            float r = RatioOf(p);
            if (r >= insideRatio) InsideCount++;
            detail += $" {p.name}={r:0.00}";
        }

        bool nowAllInside = InsideCount == pieces.Count;
        if (nowAllInside != allInside)             // chỉ đổi màu khi trạng thái thay đổi
        {
            allInside = nowAllInside;
            SetGuideColor(allInside);
        }

        stillTimer = allInside ? stillTimer + Time.deltaTime : 0f;

        if (debugLog && (InsideCount != lastInside || Time.time >= nextLog))
        {
            lastInside = InsideCount;
            nextLog = Time.time + 1f;
            Debug.Log($"[FillZone] mảnh={pieces.Count} vào={InsideCount} timer={stillTimer:0.0}/{stillTime} |{detail}");
        }

        if (stillTimer >= stillTime)
        {
            Debug.Log("[FillZone] WIN");
            IsStops = true;

            ServerScriptService.Instance.GetComponent<GameManager>().WinGame.Invoke();
        }
    }
}