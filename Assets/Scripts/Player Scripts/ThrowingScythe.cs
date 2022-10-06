using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingScythe : MonoBehaviour
{
    private GameObject target;
    private Rigidbody2D scytheRB;
    public float projectileSpeed = 5f;
    private int health = 3;
    private Animator anim;

    void Start()
    {
        scytheRB = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        GameObject[] allTargets = GameObject.FindGameObjectsWithTag("Enemy");
        if (allTargets != null)
        {
            target = allTargets[0];

            //look for the closest
            foreach (GameObject tmpTarget in allTargets)
            {
                if (Vector2.Distance(transform.position, tmpTarget.transform.position) < Vector2.Distance(transform.position, target.transform.position))
                {
                    target = tmpTarget;
                }
            }
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        Vector2 direction = target.transform.position - transform.position;
        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, projectileSpeed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            health -= 1;
        }
    }
}