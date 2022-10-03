using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingAlien : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private int soulsWorth = 1;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D alienRB;
    [SerializeField] private Collider2D alienCollider;
    [SerializeField] private GameObject soulsObj;

    private GameObject playerObj;
    private float distance;
    private Vector2 direction;
    private Animator anim;

    public float radius;

    private void Awake()
    {
        LevelWin.enemiesLeft++;
    }

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        alienCollider = GetComponent<Collider2D>();
        alienRB = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    void Update()
    {

        //Rotating enemy to face towards the player and moving it to the player
        //Vector2 direction = playerObj.transform.position - transform.position;
        //direction.Normalize();
        //float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);
        //transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        distance = Vector2.Distance(transform.position, playerObj.transform.position);

        direction = (playerObj.transform.position - transform.position).normalized;
        anim.SetFloat("Move X", direction.x);

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

    //Do something when colliding with the player
    void OnCollisionEnter2D(Collision2D coll)
    {
        //If enemy crashed into player, player takes damage then destroy this enemy
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().TakeDamage(1);
            LevelWin.enemiesLeft--;
            Destroy(gameObject);
        }
    }

    //Using the interface, takes damage from wherever the source is
    public void Damage(int damageAmount)
    {
        Debug.Log("TRACKING ALIEN HIT");
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}