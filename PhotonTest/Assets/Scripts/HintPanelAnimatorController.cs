using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintPanelAnimatorController : MonoBehaviour
{
    
    private Animator animator;
    public bool canToggle = true;
    
    void Start()
    {
        animator = this.GetComponent<Animator>();
    }
    
    public void ChangeStateOfAnimator()
    {
        if (canToggle){
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
}
