
using UnityEngine;

public class CustomBullet : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosion;
    public LayerMask WhatIsEnemies;

    // Stats
    [Range(0f, 1f)]
    public float bounciness;
    public bool useGravity;

    // Damage
    public int explosionDamage;
    public float explosionRange;

    // End-of-life
    public int maxCollisions;
    public float maxLifetime;
    public bool explodeOnTouch = true;

    int collisions;
    PhysicMaterial physics_mat;

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        // When to explode?
        // Too many collisions
        if (collisions > maxCollisions) Explode();
        
        // Too long
        maxLifetime -= Time.deltaTime;
        if (maxLifetime <= 0) Explode();
    }

    // Utilities
    private void OnCollisionEnter(Collision collision)
    {
        collisions++;

        if (collision.collider.CompareTag("Player") && explodeOnTouch) Explode();
    }

    private void Explode()
    {
        // Instantiate an explosion
        if (explosion != null) Instantiate(explosion, transform.position, Quaternion.identity);

        Debug.Log("Explode!");
        // Check for player
        Collider[] enemies = Physics.OverlapSphere(transform.position, explosionRange, WhatIsEnemies);
        // for (int i = 0; i < enemies.Length; i++)
        // {
        //     enemies[i].GetComponent<Player>().TakeDamage(explosionDamage);
        // }
        /*
        public void TakeDamage(int damage)
        {
            health -= damage;

            if (health < 0)
            {
                isDead = true;
            }
        }
        */
        // Destroy bullet
        Invoke("Delay", 0.05f);
    }

    private void Delay()
    {
        Destroy(gameObject);
    }

    private void Setup()
    {
        physics_mat = new PhysicMaterial();
        physics_mat.bounciness = bounciness;
        physics_mat.frictionCombine = PhysicMaterialCombine.Minimum;
        physics_mat.bounceCombine = PhysicMaterialCombine.Maximum;

        GetComponent<SphereCollider>().material = physics_mat;

        rb.useGravity = useGravity;
    }

    // Visualization
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRange);

    }
}
