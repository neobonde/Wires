using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{    
    
    [SerializeField] private float moveSpeed = 40;
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
        jump = Input.GetButtonDown("Jump");
        jumping = Input.GetButton("Jump"); // This allows for some floating when the player holds down the jump button
    }


    void FixedUpdate()
    {
        controller.Move(horizontalMove * moveSpeed * Time.fixedDeltaTime, jump, jumping);
    }
}
