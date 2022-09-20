using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private Camera mainCam;

    [SerializeField] private Transform gun;
    [SerializeField] private Transform barrel;
    [SerializeField] private Bullet bullet;
    [SerializeField] private float bulletCoolDown = 0.2f;

    private bool canFireAgain = true;
    private float coolDownCounter = 0;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    void Update()
    {
        AimAndShoot();

        coolDownCounter += Time.deltaTime;
        if(coolDownCounter > bulletCoolDown)
        {
            canFireAgain = true;
        }
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        float upDownInput = Input.GetAxisRaw("Vertical");
        float leftRightInput = Input.GetAxisRaw("Horizontal");

        Vector2 movement = new Vector3(leftRightInput, upDownInput).normalized * moveSpeed;

        rigidbody2D.velocity = movement;
        
        if(movement.magnitude > 0)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void AimAndShoot()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 posScreenPoint = mainCam.WorldToScreenPoint(transform.position);
        Vector3 mouseToPlayer = (mousePos - posScreenPoint).normalized;

        float angle = Mathf.Atan2(mouseToPlayer.y, mouseToPlayer.x) * Mathf.Rad2Deg;


        if(mousePos.x < posScreenPoint.x)
        {
            transform.localScale = new Vector3(-1,1,1);
            gun.localScale = new Vector3(-1,-1,1);
        }
        else
        {
            transform.localScale = Vector3.one;
            gun.localScale = Vector3.one;
        }

        gun.rotation = Quaternion.Euler(0,0,angle);

        if(Input.GetMouseButton(0) && canFireAgain)
        {
            float baseSpeed = Vector2.Dot(rigidbody2D.velocity.normalized, posScreenPoint.normalized);
            Shoot(gun.rotation, baseSpeed * 3);
            canFireAgain = false;
            coolDownCounter = 0;

            animator.SetTrigger("shootTrigger");
        }

    }

    private void Shoot(Quaternion rotation, float baseSpeed)
    {
        Bullet bulletInstance = Instantiate(bullet, barrel.position, rotation);
        bulletInstance.Initialize(baseSpeed);
    }
}
