using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleAnimationController : MonoBehaviour
{
    [SerializeField] private Animator animator;

    public void OnUse()
    {
        animator.SetBool("Deactivate", false);
        animator.SetBool("Activate", true);
    }

    public void OnLeave()
    {
        animator.SetBool("Activate", false);
        animator.SetBool("Deactivate", true);
    }
}
