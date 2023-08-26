using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpPower = 8f;
    private float horizontalInput;
    private bool isFacingRight = true;
    //public for prototyping
    public Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;

    //for combat animation handling
    public bool isAttacking;
    public static PlayerMovement instance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        isAttacking = false;
        instance = this;
    }
    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Jump();
//via direct manipulation
//            rb.velocity = new Vector3(rb.velocity.x, jumpingPower, 0);        }
        }
//        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
//        {
//            rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * 0.5f, 0);
//        }
        Attack();
        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(horizontalInput * speed, rb.velocity.y, 0);
                //set animator params
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", IsGrounded());
    
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
    }

    private bool IsGrounded()
    {
        // if raycast hits we are grounded
        // at a later date, layer will need to be added for grounded/wallgrab condition
        
        //Debug.Log(Physics.Raycast( rb.position, Vector3.down, 1.2f));
        
        return Physics.Raycast( rb.position, Vector3.down, 1.0f);
    
        //2D implementation
        //Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool OnWall()
    {
        // if raycast hits we are grabbing a wall
        // needs wall layers
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    private void Flip()
    {
        //check facing bool and input
        if (isFacingRight && horizontalInput < 0f || !isFacingRight && horizontalInput > 0f)
        {
            isFacingRight = !isFacingRight;
            Debug.Log("FACED: " + isFacingRight);
            //reflect on y axis
            Mirror();
        }
    }
    private void Mirror()
    {
        Vector3 rotationToAdd = new Vector3(0, 180, 0);
        transform.Rotate(rotationToAdd);
    }

    void Attack(){
        //use 'context' for updated input system, using raw input for now
        //Input.GetButtonDown("Fire1")
        //Input.GetKeyDown(KeyCode.A)
        if (Input.GetButtonDown("Fire1") && !isAttacking)
        {
            isAttacking = true;
        }
    }
}
