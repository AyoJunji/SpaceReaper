using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleShieldItem : MonoBehaviour
{
    [Header("Player Inventory")]
    private bool hasShield;

    [Header("Item Stuff")]
    public int shieldCost;
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
        buyItem.performed += BuyShieldAbility;
    }

    void Update()
    {
        hasShield = PlayerAbilities.hasShield;
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

    void BuyShieldAbility(InputAction.CallbackContext context)
    {
        if (soulsSO.Value >= shieldCost && !hasShield && playerInRange == true)
        {
            soulsSO.Value -= shieldCost;
        }
        else if (soulsSO.Value < shieldCost && playerInRange == true)
        {
            Debug.Log("Not enough souls!");
        }
        else if (hasShield == true && playerInRange == true)
        {
            Debug.Log("You already have a shield moron!");
        }

    }
}