using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthItem : MonoBehaviour
{
    [Header("Item Stuff")]
    public int healthItemCost;
    private bool playerInRange;

    public GameObject storeTextObj;
    public PlayerControls playerControls;
    private InputAction buyItem;

    [SerializeField] private SoulsSO soulsSO;
    [SerializeField] private HealthSO healthSO;

    void Awake()
    {
        playerControls = new PlayerControls();

    }

    void OnEnable()
    {
        buyItem = playerControls.Gameplay.Dash;
        buyItem.Enable();
        buyItem.performed += BuyHealthUpgrade;
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

    void BuyHealthUpgrade(InputAction.CallbackContext context)
    {
        if (soulsSO.Value >= healthItemCost && playerInRange == true)
        {
            soulsSO.Value -= healthItemCost;
            healthSO.MaxHealthValue += 1;
        }

        else if (healthSO.CurrentHealthValue == healthSO.MaxHealthValue)
        {
            Debug.Log("YOU HAVE MAX HEALTH DORK");
        }

        else if (soulsSO.Value < healthItemCost && playerInRange == true)
        {
            Debug.Log("Not enough souls!");
        }
    }
}