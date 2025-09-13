using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asdfa : MonoBehaviour
{
    public AnimatorOverrideController animatorOverrideController;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            animator.runtimeAnimatorController = animatorOverrideController;
        }
    }
}
