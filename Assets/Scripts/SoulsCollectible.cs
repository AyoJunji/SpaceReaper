using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulsCollectible : MonoBehaviour
{
    private GameObject playerObj;
    public float moveSpeed;
    void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(this.transform.position, playerObj.transform.position, moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            StoreManager.soulsAmount += 1;
            Destroy(gameObject);
        }
    }
}
