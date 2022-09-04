using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility : MonoBehaviour
{
    [Header("Player Inventory")]
    private bool hasDash;

    [Header("Item Stuff")]
    public int dashCost;
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
        buyItem.performed += BuyDashAbility;
    }

    void Update()
    {
        hasDash = PlayerAbilities.boughtDash;
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

    void BuyDashAbility(InputAction.CallbackContext context)
    {
        if (soulsSO.Value >= dashCost && !hasDash && playerInRange == true)
        {
            soulsSO.Value -= dashCost;
            PlayerAbilities.boughtDash = true;
        }
        else if (soulsSO.Value < dashCost && playerInRange == true)
        {
            Debug.Log("Not enough souls!");
        }
        else if (hasDash == true && playerInRange == true)
        {
            Debug.Log("You already have dash moron!");
        }

    }
}