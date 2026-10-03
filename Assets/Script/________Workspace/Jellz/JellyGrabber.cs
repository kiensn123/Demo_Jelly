using UnityEngine;

public class JellyGrabber : MonoBehaviour
{
    public Camera cam;
    public float grabRadius = 0.8f;      // bấm cách bone bao xa vẫn cầm được
    public LayerMask grabMask = ~0;      // layer của bone thạch (mặc định: tất cả)
    public float maxForce = 500f;        // lực kéo tối đa
    public float frequency = 5f;         // độ cứng "dây" kéo
    public float damping = 1f;           // hãm để khỏi rung

    [Header("Line / String Effect")]
    public LineRenderer lineRenderer;    // Kéo LineRenderer vào đây

    TargetJoint2D joint;

    void Awake() 
    { 
        if (!cam) cam = Camera.main; 

        // Nếu chưa gán LineRenderer trong Inspector thì tự tạo/lấy luôn
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.enabled = false; // Mặc định ẩn dây đi
        }
    }

    void Update()
    {
        Vector2 p = cam.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0)) TryGrab(p);

        if (joint && Input.GetMouseButton(0))
        {
            joint.target = p;

            // Cập nhật vị trí 2 đầu dây khi đang kéo
            if (lineRenderer)
            {
                // Điểm 1: Điểm neo chính xác trên bone thạch
                Vector3 anchorWorldPos = joint.transform.TransformPoint(joint.anchor);
                lineRenderer.SetPosition(0, anchorWorldPos);

                // Điểm 2: Vị trí con trỏ chuột/tay
                lineRenderer.SetPosition(1, new Vector3(p.x, p.y, anchorWorldPos.z));
            }
        }

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

        // Hiện dây nối
        if (lineRenderer) lineRenderer.enabled = true;
    }

    void Release()
    {
        if (joint) Destroy(joint);
        joint = null;

        // Ẩn dây khi thả tay
        if (lineRenderer) lineRenderer.enabled = false;
    }
}