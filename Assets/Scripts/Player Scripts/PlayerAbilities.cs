using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Purchased Items")]
    public static bool boughtDash;
    public static bool hasMaxNukes;
    public static bool boughtScytheThrow;
    public static bool hasShield;
    private int shieldCheck;

    [Header("Abilities")]
    public static int currentNukes;
    public int maxNukes = 5;
    public GameObject bubbleShield;

    void Update()
    {
        if (currentNukes == maxNukes)
        {
            hasMaxNukes = true;
        }

        else if (currentNukes < maxNukes)
        {
            hasMaxNukes = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            GameObject objReference = coll.gameObject;
            Destroy(objReference);
            bubbleShield.SetActive(false);
        }
    }
}