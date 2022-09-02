using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScytheAttack : MonoBehaviour
{
    public Collider2D scytheColl;
    Vector2 rightAttackOffset;
    public int scytheDamage = 1;

    void Start()
    {
        rightAttackOffset = transform.position;
    }

    public void AttackRight()
    {
        scytheColl.enabled = true;
        transform.localPosition = rightAttackOffset;

    }

    public void AttackLeft()
    {
        scytheColl.enabled = true;
        transform.localPosition = new Vector3(rightAttackOffset.x * -1, rightAttackOffset.y);

    }

    public void StopAttack()
    {
        scytheColl.enabled = false;

    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            IDamageable damageable = coll.gameObject.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                Debug.Log("HIT ENEMY");
                damageable.Damage(scytheDamage);
            }
        }
    }
}