using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAlien : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float cooldown = 1f;
    [SerializeField] private int soulsWorth = 3;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D alienRB;
    [SerializeField] private Collider2D alienCollider;
    [SerializeField] private GameObject soulsObj;
    [SerializeField] private AudioSource audioSource;
    public AudioClip shootNoise;
    public Transform mouthPosition;
    private GameObject playerObj;
    public GameObject projectile;
    Animator anim;
    private Vector2 direction;

    [Header("Range")]
    private float distance;
    [SerializeField] private float distanceBetween;

    private bool projectileResetted;
    public float radius;

    private void Awake()
    {
        LevelWin.enemiesLeft++;
    }

    private void Start()
    {
        health = 1;
        playerObj = GameObject.FindGameObjectWithTag("Player");
        projectileResetted = true;
        alienCollider = GetComponent<Collider2D>();
        alienRB = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        distance = Vector2.Distance(transform.position, playerObj.transform.position);

        direction = (playerObj.transform.position - transform.position).normalized;
        anim.SetFloat("Move X", direction.x);

        //Rotating enemy to face towards the player and moving it to the player
        //Vector2 direction = playerObj.transform.position - transform.position;
        //direction.Normalize();
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        //Minimum distance before the alien can shoot at the player
        if (distance < distanceBetween)
        {
            if (projectileResetted == true)
            {
                if (shootNoise != null)
                {
                    audioSource.PlayOneShot(shootNoise);
                }
                ShootProjectile();
                StartCoroutine(ProjectileCooldown());
            }
        }

        if (health <= 0)
        {
            for (var i = 0; i < soulsWorth; i++)
            {
                Vector3 randomPos = Random.insideUnitCircle * radius;
                Instantiate(soulsObj, this.transform.position + randomPos, Quaternion.identity);
            }

            LevelWin.enemiesLeft--;
            Destroy(gameObject);
        }
    }

    //Spawning projectile then putting it in cooldown
    void ShootProjectile()
    {
        Instantiate(projectile, mouthPosition.position, Quaternion.identity);
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
        Debug.Log("SHOOTING ALIEN HIT");
        health -= damageAmount;
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Nuke" || coll.gameObject.tag == "Scythe")
        {
            Instantiate(soulsObj, this.transform.position, Quaternion.identity);
            LevelWin.enemiesLeft--;
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        //If enemy crashed into player, player takes damage then destroy this enemy
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().TakeDamage(1);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}