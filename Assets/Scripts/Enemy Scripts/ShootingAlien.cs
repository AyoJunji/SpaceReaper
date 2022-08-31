using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAlien : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float cooldown = 1f;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D alienRB;
    [SerializeField] private Collider2D alienCollider;
    public GameObject playerObj;
    public GameObject projectile;

    [Header("Range")]
    private float distance;
    [SerializeField] private float distanceBetween;

    private bool projectileResetted;

    private void Start()
    {
        projectileResetted = true;
        alienCollider = GetComponent<Collider2D>();
        alienRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, playerObj.transform.position);

        //Rotating enemy to face towards the player and moving it to the player
        Vector2 direction = playerObj.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        //Minimum distance before the alien can shoot at the player
        if (distance < distanceBetween)
        {
            if (projectileResetted == true)
            {
                ShootProjectile();
                StartCoroutine(ProjectileCooldown());
            }
        }
    }

    //Spawning projectile then putting it in cooldown
    void ShootProjectile()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
        projectileResetted = false;
    }

    //Cooldown for projectile
    IEnumerator ProjectileCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        projectileResetted = true;
    }

    //Destroying the alien object only when hit by the player
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "Friendly Projectiles")
        {
            Destroy(gameObject);
        }
    }
}
