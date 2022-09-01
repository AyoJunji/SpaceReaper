using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretProjectile : MonoBehaviour
{
    [Header("Stats")]
    public float projectileSpeed;

    private Rigidbody2D projectileRB;
    private Transform playerObj;
    private Vector2 targetPlayer;

    private void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player").transform;
        projectileRB = GetComponent<Rigidbody2D>();


        targetPlayer = (playerObj.transform.position - transform.position).normalized * projectileSpeed;


        Destroy(gameObject, 4f);
    }

    private void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, playerObj.position, projectileSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponentInParent<PlayerController>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
