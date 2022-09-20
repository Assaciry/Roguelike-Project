using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class EnemyController : MonoBehaviour, IDamagable
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float health = 150f;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private Vector3 moveDirection;
    
    public float playerRadius = 10.0f;
    private Transform playerTransform;
    private Vector3 playerLastPos;

    [SerializeField] private CircleDrawer drawer;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        playerTransform = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        
        drawer.radius = playerRadius;

    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerIsInRadius();
        
        #if UNITY_EDITOR
        // Ensure continuous Update calls.
         if (!Application.isPlaying)
        {
            drawer.radius = playerRadius;
        }
        #endif
    }

    void CheckPlayerIsInRadius()
    {
        if(Vector3.Distance(transform.position, playerTransform.position) <= playerRadius)
        {
            moveDirection = playerTransform.position - transform.position;
            playerLastPos = playerTransform.position;

            if(moveDirection.x > 0)
            {
                transform.localScale = new Vector3(-1, 1,1);
            }
            else
            {
                transform.localScale = Vector3.one;
            }

            animator.SetBool("isWalking", true);
        }

        else
        {
            if(Vector3.Distance(transform.position, playerLastPos) <= 1)
            {
                moveDirection = Vector3.zero;

               animator.SetBool("isWalking", false);
            }
        }

        moveDirection.Normalize();

        rigidbody2D.velocity = moveDirection * moveSpeed;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;

        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

}
