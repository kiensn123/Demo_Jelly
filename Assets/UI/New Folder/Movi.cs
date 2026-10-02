using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movi : MonoBehaviour
{
   public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Lấy input từ bàn phím
        float moveX = Input.GetAxisRaw("Horizontal"); // A/D hoặc ←/→
        float moveY = Input.GetAxisRaw("Vertical");   // W/S hoặc ↑/↓
        moveInput = new Vector2(moveX, moveY).normalized;
    }

    void FixedUpdate()
    {
        // Di chuyển bằng vận tốc Rigidbody2D
        rb.linearVelocity = moveInput * moveSpeed;
    }
}
