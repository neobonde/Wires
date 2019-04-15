using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    [SerializeField] private Animator animator;    
    private PlayerController controller;
    private float horizontalMove = 0f;
    private bool jump = false;
    private bool jumping = false;
    
    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        if(Input.GetButtonDown("Jump"))
            jump = true;
        jumping = Input.GetButton("Jump"); // This allows for some floating when the player holds down the jump button

        Animate();

    }


    void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump, jumping);
        if(jump)
            jump = false;
    }

    void Animate()
    {
        animator.SetBool("Moving",controller.GetMoving());
        // animator.SetBool("Jumping", controller.GetJumping());
        // animator.SetBool("Jumping", !controller.GetGrounded());
        if(controller.GetJumping())
            animator.SetBool("Jumping", true);
        if(controller.GetGrounded())
            animator.SetBool("Jumping",false);
    }

}
