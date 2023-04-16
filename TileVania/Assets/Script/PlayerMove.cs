using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMove : MonoBehaviour
{

    Vector2 moveInput;
    [SerializeField] float speed= 7f;
    [SerializeField] float jumpSpeed = 7f;
    [SerializeField] float climbSpeed = 6f;
    [SerializeField] Vector2 deathkick = new Vector2(10f,10f);
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    Rigidbody2D rb2d;
    Animator animator;
    CapsuleCollider2D cc2d;
    BoxCollider2D bc2d;
    float gratvityScaleStart;

    bool isAlive = true;
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        cc2d = GetComponent<CapsuleCollider2D>();
        bc2d = GetComponent<BoxCollider2D>();
        gratvityScaleStart = rb2d.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isAlive){return;}
        Run();
        FlipSprite();
        ClimbLabor();
        die();
    }

    void OnMove(InputValue value)
    {
        if(!isAlive){return;}
        moveInput = value.Get<Vector2>();

    }
    
    void OnJump(InputValue value)
    {
        if(!isAlive){return;}
        if(!bc2d.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if(value.isPressed)
        {
            rb2d.velocity += new Vector2 (0f, jumpSpeed);
        }
    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2d.velocity.y);
        rb2d.velocity = playerVelocity;
        bool playerHasHorizonTalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        
        animator.SetBool("isRuning",playerHasHorizonTalSpeed);

    }

    void FlipSprite()
    {
        bool playerHasHorizonTalSpeed = Mathf.Abs(rb2d.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizonTalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rb2d.velocity.x),1f);
        }
    }

    void ClimbLabor()
    {
        if(!bc2d.IsTouchingLayers(LayerMask.GetMask("climbing")))
        {
            rb2d.gravityScale = gratvityScaleStart;
            animator.SetBool("isClimbing", false);
            return;
        }

        Vector2 climbVelocity = new Vector2(rb2d.velocity.x, moveInput.y * climbSpeed);
        rb2d.velocity = climbVelocity;
        rb2d.gravityScale = 0;

        bool playerHasHorizonTalSpeed = Mathf.Abs(rb2d.velocity.y) > Mathf.Epsilon;
        
        animator.SetBool("isClimbing", playerHasHorizonTalSpeed);
    }

    void die()
    {
        if(rb2d.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            animator.SetTrigger("dying");
            rb2d.velocity = deathkick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

    void OnFire(InputValue value)
    {
        if(!isAlive){return;}
        Instantiate(bullet, gun.position, transform.rotation);
    }
}
