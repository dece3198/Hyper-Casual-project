using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<GroundChecker>() != null)
        {
            if(other.GetComponent<GroundChecker>().isOpen)
            {
                animator.SetBool("OpenA", true);
            }
            else
            {
                animator.SetBool("OpenB", true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        animator.SetBool("OpenA", false);
        animator.SetBool("OpenB", false);
    }
}
