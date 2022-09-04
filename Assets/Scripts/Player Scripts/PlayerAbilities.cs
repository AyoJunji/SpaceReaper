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
}