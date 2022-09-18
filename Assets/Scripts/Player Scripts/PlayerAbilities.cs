using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Purchased Items")]
    public static bool hasMaxNukes;

    [Header("Abilities")]
    public static int currentNukes;
    public int maxNukes = 5;
    public GameObject bubbleShield;

    [Header("Player Input")]
    [SerializeField] public PlayerControls playerControls;
    private InputAction playerDash;

    [SerializeField] private AbilitiesSO abilitiesSO;

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

        if (abilitiesSO.CheckBubbleShield == true)
        {
            bubbleShield.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Enemy")
        {
            abilitiesSO.CheckBubbleShield = false;
            GameObject objReference = coll.gameObject;
            Destroy(objReference);
            bubbleShield.SetActive(false);
        }
    }
}