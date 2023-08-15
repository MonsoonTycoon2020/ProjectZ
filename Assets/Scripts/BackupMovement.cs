using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupMovement : MonoBehaviour
{
    // Update is called once per frame
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 8f;
    private bool isFacingRight = true;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y, 0);
    }

    private bool IsGrounded()
    {
        //If raycast hits we are grounded
        //Debug.Log(Physics.Raycast( rb.position, Vector3.down, 1.2f));
        return Physics.Raycast( rb.position, Vector3.down, 1.2f);
    
        //2D implementation
        //Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
      //      localScale.x *= -1f;
            Debug.Log(localScale);
            transform.localScale = localScale;
        }
    }
}
