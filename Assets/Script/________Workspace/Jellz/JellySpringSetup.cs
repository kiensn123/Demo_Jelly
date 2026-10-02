using System.Collections.Generic;
using UnityEngine;

public class JellySpringSetup : MonoBehaviour
{
    public List<Rigidbody2D> rim;     // kéo 8 bone viền vào, đúng thứ tự vòng quanh
    public Rigidbody2D center;        // bone tâm (để trống nếu không có)
    public float frequency = 4f;
    public float damping = 0.4f;
    public bool connectSkipOne = true; // nối cách một bone, chống gập

    [ContextMenu("Setup Springs")]


    void Start()
    {
        Setup();
    }
    void Setup()
    {
        // xóa joint cũ để chạy lại không bị nhân đôi
        foreach (var j in GetComponentsInChildren<SpringJoint2D>())
            DestroyImmediate(j);

        int n = rim.Count;
        for (int i = 0; i < n; i++)
        {
            Connect(rim[i], rim[(i + 1) % n]);                    // bone kề
            if (connectSkipOne) Connect(rim[i], rim[(i + 2) % n]); // cách một bone
            if (center) Connect(rim[i], center);                   // nối vào tâm
        }
    }

    void Connect(Rigidbody2D a, Rigidbody2D b)
    {
        var j = a.gameObject.AddComponent<SpringJoint2D>();
        j.connectedBody = b;
        j.autoConfigureDistance = true;   // lấy khoảng cách hiện tại làm độ dài nghỉ
        j.enableCollision = false;        // hai bone nối nhau không va chạm
        j.frequency = frequency;
        j.dampingRatio = damping;
    }
}