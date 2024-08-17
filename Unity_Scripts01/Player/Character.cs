using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private float speed;
    protected Vector2 direction;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        Move();
        AnimateMovement();
    }

    public void Move()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    public void AnimateMovement()
    {
        //Sets the animation parameter so that he faces the correct direction
        animator.SetFloat("x", direction.x);
        animator.SetFloat("y", direction.y);
    }
}
 
