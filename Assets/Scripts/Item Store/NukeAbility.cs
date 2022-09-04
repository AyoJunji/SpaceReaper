using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NukeAbility : MonoBehaviour
{
    [Header("Player Inventory")]
    private bool hasMaxNukes;

    [Header("Item Stuff")]
    public int nukeCost;
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
        buyItem.performed += BuyNukeAbility;
    }

    void Update()
    {
        hasMaxNukes = PlayerAbilities.hasMaxNukes;
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

    void BuyNukeAbility(InputAction.CallbackContext context)
    {
        if (soulsSO.Value >= nukeCost && !hasMaxNukes && playerInRange == true)
        {
            soulsSO.Value -= nukeCost;
            PlayerAbilities.currentNukes += 1;
        }
        else if (soulsSO.Value < nukeCost && playerInRange == true)
        {
            Debug.Log("Not enough souls!");
        }
        else if (hasMaxNukes == true && playerInRange == true)
        {
            Debug.Log("You already have max nukes moron!");
        }

    }
}