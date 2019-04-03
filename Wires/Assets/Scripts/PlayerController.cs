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


    [Header("Events")]
	[Space]
	public UnityEvent OnMoveEvent;
	public UnityEvent OnStopEvent;
	public UnityEvent OnJumpEvent;
	public UnityEvent OnLandEvent;


    const float groundedRadius = .2f;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;
    private bool grounded;
    private bool facingRight = true;

    void Awake()
    {
        rb = transform.GetComponent<Rigidbody2D>();

        SetupEvents();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement= Vector2.right * Input.GetAxis("Horizontal");
    }

    void FixedUpdate()
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position,groundedRadius,groundLayers);
        foreach (Collider2D collider in colliders)
        {
            if(collider.gameObject != gameObject)
            {
                grounded = true;
                if(!wasGrounded && rb.velocity.y < 0)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
        

    }


    public void Move(float move, bool jump, bool jumping)
    {

        Vector3 targetVelocity = new Vector2(move * 10f, rb.velocity.y);
        // And then smoothing it out and applying it to the character
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, movementSmoothing);

        if(move == 0)
        {
            OnStopEvent.Invoke();    
        }else
        {
            OnMoveEvent.Invoke();
        }

        if(move > 0 && !facingRight)
        {
            Flip();
        }
        else if (move < 0 && facingRight)
        {
            Flip();
        }


        // If the player should jump...
		if (grounded && jump)
		{
			// Add a vertical force to the player.
			grounded = false;
			rb.AddForce(new Vector2(0f, m_JumpForce));
            OnJumpEvent.Invoke();
		}

        if(rb.velocity.y < 0 )
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * ( fallMultiplier - 1 ) * Time.fixedDeltaTime;
        }else if (rb.velocity.y > 0 && !jumping)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * ( slowJumpMultiplier - 1 ) * Time.fixedDeltaTime; 
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void SetupEvents()
    {
        if (OnMoveEvent == null)
            OnMoveEvent = new UnityEvent();

        if (OnStopEvent == null)
			    OnStopEvent = new UnityEvent();

        if (OnJumpEvent == null)
			    OnJumpEvent = new UnityEvent();

        if (OnLandEvent == null)
			    OnLandEvent = new UnityEvent();
    }

}
