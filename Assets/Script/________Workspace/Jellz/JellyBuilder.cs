using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class JellyBuilder : MonoBehaviour
{
    public float spacing = 0.35f;            // khoảng cách giữa các điểm
    public float frequency = 4f, damping = 0.4f;
    public float pointRadius = 0.12f;

    List<Vector2> outline;                   // viền (world space)
    readonly List<Rigidbody2D> points = new();
    readonly List<(int, int)> links = new(); // để vẽ Gizmos

    [ContextMenu("Build")]
    public void Build()
    {
        var col = GetComponent<PolygonCollider2D>();
        outline = Resample(col.GetPath(0), spacing, transform);
        col.enabled = false;                 // dùng điểm thay cho collider gốc

        var pos = new List<Vector2>(outline);
        pos.AddRange(InteriorPoints(outline, spacing));  // xương bên trong

        foreach (var p in pos) points.Add(CreatePoint(p));

        for (int i = 0; i < pos.Count; i++)
            for (int j = i + 1; j < pos.Count; j++)
            {
                if (Vector2.Distance(pos[i], pos[j]) > spacing * 1.5f) continue;
                if (!Inside((pos[i] + pos[j]) * 0.5f, outline)) continue; // không nối qua chỗ lõm
                var sj = points[i].gameObject.AddComponent<SpringJoint2D>();
                sj.connectedBody = points[j];
                sj.autoConfigureDistance = true;
                sj.enableCollision = false;
                sj.frequency = frequency;
                sj.dampingRatio = damping;
                links.Add((i, j));
            }
    }

    Rigidbody2D CreatePoint(Vector2 p)
    {
        var go = new GameObject("JellyPoint");
        go.transform.SetParent(transform);
        go.transform.position = p;
        go.AddComponent<CircleCollider2D>().radius = pointRadius;
        return go.AddComponent<Rigidbody2D>();
    }

    // chia đều điểm trên viền
    static List<Vector2> Resample(Vector2[] local, float step, Transform t)
    {
        var res = new List<Vector2>();
        float carry = 0;
        for (int i = 0; i < local.Length; i++)
        {
            Vector2 a = t.TransformPoint(local[i]);
            Vector2 b = t.TransformPoint(local[(i + 1) % local.Length]);
            float len = Vector2.Distance(a, b), d = carry;
            while (d < len) { res.Add(Vector2.Lerp(a, b, d / len)); d += step; }
            carry = d - len;
        }
        return res;
    }

    // rải lưới điểm bên trong, bỏ điểm sát viền
    static List<Vector2> InteriorPoints(List<Vector2> poly, float step)
    {
        var res = new List<Vector2>();
        Vector2 min = poly[0], max = poly[0];
        foreach (var p in poly) { min = Vector2.Min(min, p); max = Vector2.Max(max, p); }
        for (float x = min.x; x <= max.x; x += step)
            for (float y = min.y; y <= max.y; y += step)
            {
                var p = new Vector2(x, y);
                if (!Inside(p, poly)) continue;
                bool nearEdge = false;
                foreach (var q in poly)
                    if (Vector2.Distance(p, q) < step * 0.7f) { nearEdge = true; break; }
                if (!nearEdge) res.Add(p);
            }
        return res;
    }

    static bool Inside(Vector2 p, List<Vector2> poly)   // point-in-polygon
    {
        bool c = false;
        for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            if ((poly[i].y > p.y) != (poly[j].y > p.y) &&
                p.x < (poly[j].x - poly[i].x) * (p.y - poly[i].y) / (poly[j].y - poly[i].y) + poly[i].x)
                c = !c;
        return c;
    }

    void OnDrawGizmos()                                  // xem xương khi chạy
    {
        Gizmos.color = Color.cyan;
        foreach (var (a, b) in links)
            Gizmos.DrawLine(points[a].position, points[b].position);
    }
}