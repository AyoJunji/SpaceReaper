using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScytheThrowAbility : MonoBehaviour
{
    [Header("Player Inventory")]
    private bool hasScytheThrow;

    [Header("Item Stuff")]
    public int scytheThrowCost;
    private bool playerInRange;

    public GameObject storeTextObj;
    public PlayerControls playerControls;
    private InputAction buyItem;

    [SerializeField]
    private SoulsSO soulsSO;
    void Awake()
    {
        playerControls = new PlayerControls();

    }

    void OnEnable()
    {
        buyItem = playerControls.Gameplay.Dash;
        buyItem.Enable();
        buyItem.performed += BuyThrowAbility;
    }

    void Update()
    {
        hasScytheThrow = PlayerAbilities.boughtScytheThrow;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            storeTextObj.SetActive(true);
            playerInRange = true;
        }
    }
    void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            storeTextObj.SetActive(false);
            playerInRange = false;
        }
    }

    void BuyThrowAbility(InputAction.CallbackContext context)
    {
        if (soulsSO.Value >= scytheThrowCost && !hasScytheThrow && playerInRange == true)
        {
            soulsSO.Value -= scytheThrowCost;
            PlayerAbilities.boughtScytheThrow = true;
        }
        else if (soulsSO.Value < scytheThrowCost && playerInRange == true)
        {
            Debug.Log("Not enough souls!");
        }
        else if (hasScytheThrow == true && playerInRange == true)
        {
            Debug.Log("You already have scythe throw moron!");
        }

    }
}