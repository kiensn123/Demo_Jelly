using UnityEngine;

public class JellyGrabber : MonoBehaviour
{
    public Camera cam;
    public float grabRadius = 0.8f;      // bấm cách bone bao xa vẫn cầm được
    public LayerMask grabMask = ~0;      // layer của bone thạch (mặc định: tất cả)
    public float maxForce = 500f;        // lực kéo tối đa
    public float frequency = 5f;         // độ cứng "dây" kéo
    public float damping = 1f;           // hãm để khỏi rung

    TargetJoint2D joint;

    void Awake() { if (!cam) cam = Camera.main; }

    void Update()
    {
        Vector2 p = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) TryGrab(p);
        if (joint && Input.GetMouseButton(0)) joint.target = p;
        if (Input.GetMouseButtonUp(0)) Release();
    }

    void TryGrab(Vector2 p)
    {
        Rigidbody2D best = null;
        float bestDist = float.MaxValue;

        foreach (var c in Physics2D.OverlapCircleAll(p, grabRadius, grabMask))
        {
            var rb = c.attachedRigidbody;
            if (!rb || rb.bodyType != RigidbodyType2D.Dynamic) continue;
            float d = ((Vector2)rb.position - p).sqrMagnitude;
            if (d < bestDist) { bestDist = d; best = rb; }
        }
        if (!best) return;

        joint = best.gameObject.AddComponent<TargetJoint2D>();
        joint.autoConfigureTarget = false;
        joint.anchor = best.transform.InverseTransformPoint(p);   // cầm đúng điểm vừa bấm
        joint.target = p;
        joint.maxForce = maxForce;
        joint.frequency = frequency;
        joint.dampingRatio = damping;
    }

    void Release()
    {
        if (joint) Destroy(joint);
        joint = null;
    }
}