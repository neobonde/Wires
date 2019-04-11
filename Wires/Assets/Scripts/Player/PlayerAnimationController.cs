using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public PlayerAnimationController()
    {
    }

    public void OnMove()
    {
        animator.SetBool("Moving",true);
    }

    public void OnStop()
    {
        animator.SetBool("Moving",false);
    }

    public void OnJump()
    {
        animator.SetBool("Jumping", true);
    }

    public void OnLand()
    {
        animator.SetBool("Jumping", false);
    }
}
