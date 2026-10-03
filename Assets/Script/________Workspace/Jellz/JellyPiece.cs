// JellyPiece.cs
using System.Collections.Generic;
using UnityEngine;

public class JellyPiece : MonoBehaviour
{
    public List<Rigidbody2D> bones = new();   // để trống thì tự gom tất cả bone con

    void Awake()
    {
        if (bones.Count == 0)
            bones.AddRange(GetComponentsInChildren<Rigidbody2D>());
    }

    // tỉ lệ bone nằm trong vùng đạt mức yêu cầu thì mảnh coi là đã vào
    public bool IsInside(Collider2D zone, float requiredRatio)
    {
        int inside = 0;
        foreach (var b in bones)
            if (zone.OverlapPoint(b.position)) inside++;
        return (float)inside / bones.Count >= requiredRatio;
    }

    public bool IsStill(float speed)
    {
        foreach (var b in bones)
            if (b.linearVelocity.magnitude > speed) return false;   // Unity cũ: b.velocity
        return true;
    }
}