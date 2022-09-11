using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienTurret : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float cooldown = 1.7f;
    [SerializeField] private int soulsWorth = 2;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D alienRB;
    [SerializeField] private Collider2D alienCollider;
    [SerializeField] private GameObject soulsObj;
    [SerializeField] private AudioSource audioSource;
    public AudioClip shootNoise;
    public Transform barrelPosition;

    private GameObject playerObj;
    public GameObject projectile;

    [Header("Range")]
    private float distance;
    [SerializeField] private float distanceBetween;

    private bool projectileResetted;
    public float radius;

    private void Start()
    {
        projectileResetted = true;
        alienCollider = GetComponent<Collider2D>();
        alienRB = GetComponent<Rigidbody2D>();
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, playerObj.transform.position);

        //Rotating enemy to face towards the player and moving it to the player
        Vector2 direction = playerObj.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        //Minimum distance before the alien can shoot at the player
        if (distance < distanceBetween)
        {
            if (projectileResetted == true)
            {
                audioSource.PlayOneShot(shootNoise);
                ShootProjectile();
                StartCoroutine(ProjectileCooldown());
            }
        }

        if (health <= 0)
        {
            for (var i = 0; i < soulsWorth; i++)
            {
                Vector3 randomPos = Random.insideUnitCircle * radius;
                Instantiate(soulsObj, transform.position + randomPos, Quaternion.identity);
            }

            Destroy(gameObject);
        }
    }

    //Spawning projectile then putting it in cooldown
    void ShootProjectile()
    {
        Instantiate(projectile, barrelPosition.position, Quaternion.identity);
        projectileResetted = false;
    }

    //Cooldown for projectile
    IEnumerator ProjectileCooldown()
    {
        yield return new WaitForSeconds(cooldown);
        projectileResetted = true;
    }

    //Using the interface, takes damage from wherever the source is
    public void Damage(int damageAmount)
    {
        Debug.Log("ALIEN TURRET HIT");
        health -= damageAmount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
