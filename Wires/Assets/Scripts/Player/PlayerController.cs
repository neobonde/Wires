using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float movementSpeed = 500;

    [SerializeField] private float m_JumpForce = 400f;
    [Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayers;
    [Range(0,10f)] [SerializeField] private float fallMultiplier = 5f;
    [Range(0,10)] [SerializeField] public float slowJumpMultiplier = 2.0f;

    const float groundedRadius = .1f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private bool facingRight = true;
    private bool _jumpFloat = false;
    private bool _jump = false;

    private bool _grounded;
    private bool _jumping;
    private bool _moving;


    void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement= Vector2.right * Input.GetAxis("Horizontal");
        // if(landed){
        //     landed = false;
            
        // }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position,groundedRadius);
    }

    void FixedUpdate()
    {
        bool wasGrounded = _grounded;
        _grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position,groundedRadius,groundLayers);
        foreach (Collider2D collider in colliders)
        {
            if((collider.gameObject != gameObject) && !collider.isTrigger)
            {
                _grounded = true;
                if(!wasGrounded && rb.velocity.y < 0)
                {
                    _jumping = false;
                }
            }
        }

        // If the player should jump...
		if (_grounded && _jump)
		{
			// Add a vertical force to the player.
			rb.AddForce(new Vector2(0f, m_JumpForce));
			_grounded = false;
            _jumping = true;
		}
            
        _jump = false;

        if(rb.velocity.y < 0 )
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * ( fallMultiplier - 1 ) * Time.fixedDeltaTime;
        }else if (rb.velocity.y > 0 && !_jumpFloat)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * ( slowJumpMultiplier - 1 ) * Time.fixedDeltaTime; 
        }
    }


    public void Move(float move, bool jump, bool jumpFloat)
    {

        Vector3 targetVelocity = new Vector2(move * movementSpeed, rb.velocity.y);
        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);


        if(jump)
            _jump = true;
        
        _jumpFloat = jumpFloat; 


        if(move == 0)
        {
            _moving = false;    
        }else
        {
            _moving = true;
        }

        if(move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }


    public bool GetGrounded()
    {
        return _grounded;
    }

    public bool GetJumping()
    {
        return _jumping;  
    }

    public bool GetMoving()
    {
        return _moving;
    }

}
