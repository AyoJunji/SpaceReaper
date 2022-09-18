using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BubbleShieldItem : MonoBehaviour
{
    [Header("Item Stuff")]
    public int shieldCost;
    private bool playerInRange;

    public GameObject storeTextObj;
    public PlayerControls playerControls;
    private InputAction buyItem;

    [SerializeField] private SoulsSO soulsSO;
    [SerializeField] private AbilitiesSO abilitiesSO;

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
        if (soulsSO.Value >= shieldCost && abilitiesSO.CheckBubbleShield == false && playerInRange == true)
        {
            soulsSO.Value -= shieldCost;
            abilitiesSO.CheckBubbleShield = true;
        }
        else if (soulsSO.Value < shieldCost && playerInRange == true)
        {
            Debug.Log("Not enough souls!");
        }
        else if (abilitiesSO.CheckBubbleShield == true && playerInRange == true)
        {
            Debug.Log("You already have a shield moron!");
        }

    }
}