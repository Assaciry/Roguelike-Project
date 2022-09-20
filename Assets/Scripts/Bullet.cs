using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private float damage = 25f;
    [SerializeField] private GameObject hitFX;
    private Rigidbody2D rigidbody2D;
    private float baseSpeed;


    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = transform.right * (baseSpeed + speed);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.TryGetComponent<IDamagable>(out IDamagable damagable))
        {
            damagable.TakeDamage(damage);
            Instantiate(hitFX, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    public void Initialize(float baseSpeed)
    {
        this.baseSpeed = baseSpeed;
    }
}
