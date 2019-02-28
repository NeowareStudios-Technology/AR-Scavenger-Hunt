using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class modelAnimationController : MonoBehaviour
{
    private Animator animator;
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    
    public void ChangeStateOfAnimator()
    {
        if (animator.GetBool("open"))
        {
            animator.SetBool("open", false);
        }
        else
        {
            animator.SetBool("open", true);
        } 
    }
}
