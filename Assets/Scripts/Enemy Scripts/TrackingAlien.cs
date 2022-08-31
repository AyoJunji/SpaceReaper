using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackingAlien : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float moveSpeed = 8f;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D alienRB;
    [SerializeField] private Collider2D alienCollider;

    public GameObject playerObj;
    private float distance;

    private void Start()
    {
        alienCollider = GetComponent<Collider2D>();
        alienRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

        //Rotating enemy to face towards the player and moving it to the player
        Vector2 direction = playerObj.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.position = Vector2.MoveTowards(this.transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Friendly Projectiles")
        {
            Destroy(gameObject);
        }
    }
}