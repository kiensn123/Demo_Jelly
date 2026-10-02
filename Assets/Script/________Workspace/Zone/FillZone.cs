// FillZone.cs
using System;
using System.Collections.Generic;
using UnityEngine;

public class FillZone : MonoBehaviour
{
    public Collider2D zone;               // vùng cần lấp (Is Trigger)
    public int totalPieces = 3;           // số mảnh của level
    [Range(0.5f, 1f)] public float insideRatio = 1f;
    public float stillSpeed = 0.05f;
    public float stillTime = 0.5f;
    public bool debugLog = true;

    readonly List<JellyPiece> pieces = new();
    float stillTimer;
    bool won;
    int lastInside = -1;
    float nextLog;

    public int InsideCount { get; private set; }
    public event Action OnWin;

    public void Register(JellyPiece p)
    {
        if (!pieces.Contains(p)) pieces.Add(p);
        if (debugLog) Debug.Log($"[FillZone] Đăng ký {p.name} ({pieces.Count}/{totalPieces})");
    }

    public void Unregister(JellyPiece p)
    {
        pieces.Remove(p);
        if (debugLog) Debug.Log($"[FillZone] Gỡ {p.name}");
    }

    void Start()
    {
        if (!zone) { Debug.LogError("[FillZone] Chưa gán ô Zone"); enabled = false; return; }
        if (debugLog) Debug.Log($"[FillZone] Sẵn sàng. Zone={zone.name}, isTrigger={zone.isTrigger}, bounds={zone.bounds}");
    }

    // tỉ lệ bone của mảnh nằm trong vùng (tính ngay tại đây, không phụ thuộc hàm khác)
    float RatioOf(JellyPiece p)
    {
        if (p.bones.Count == 0) return 0f;
        int inside = 0;
        foreach (var b in p.bones)
            if (zone.OverlapPoint(b.position)) inside++;
        return (float)inside / p.bones.Count;
    }

    void Update()
    {
        if (won || pieces.Count == 0) return;

        InsideCount = 0;
        bool allStill = true;
        string detail = "";

        foreach (var p in pieces)
        {
            float r = RatioOf(p);
            if (r >= insideRatio) InsideCount++;
            if (!p.IsStill(stillSpeed)) allStill = false;
            detail += $" {p.name}={r:0.00}";
        }

        bool ready = pieces.Count >= totalPieces && InsideCount == pieces.Count && allStill;
        stillTimer = ready ? stillTimer + Time.deltaTime : 0f;

        if (debugLog && (InsideCount != lastInside || Time.time >= nextLog))
        {
            lastInside = InsideCount;
            nextLog = Time.time + 1f;
            Debug.Log($"[FillZone] mảnh={pieces.Count}/{totalPieces} vào={InsideCount} yên={allStill} timer={stillTimer:0.0}/{stillTime} |{detail}");
        }

        if (stillTimer >= stillTime)
        {
            won = true;
            Debug.Log("[FillZone] WIN");
            OnWin?.Invoke();
        }
    }
}