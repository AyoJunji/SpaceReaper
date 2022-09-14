using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicProjectile : MonoBehaviour
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

        projectileRB.velocity = new Vector2(targetPlayer.x, targetPlayer.y);

        Vector2 direction = playerObj.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        Destroy(gameObject, 4f);
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
