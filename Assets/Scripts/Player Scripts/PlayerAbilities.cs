using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Purchased Items")]
    public static bool hasMaxNukes;

    [Header("Abilities")]
    public GameObject bubbleShield;
    public GameObject nukeBox;

    [Header("Player Input")]
    [SerializeField] public PlayerControls playerControls;
    private InputAction playerNuke;

    [SerializeField] private AbilitiesSO abilitiesSO;

    private float nukeCooldown = 3f;
    private float nextNukeTime = 0;
    private bool readyToNuke;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    void Update()
    {
        if (Time.time > nextNukeTime)
        {
            readyToNuke = false;
            nextNukeTime = Time.time + nukeCooldown;
            readyToNuke = true;
        }

        if (abilitiesSO.CurrentNukeValue == abilitiesSO.MaxNukeValue)
        {
            hasMaxNukes = true;
        }

        else if (abilitiesSO.CurrentNukeValue < abilitiesSO.MaxNukeValue)
        {
            hasMaxNukes = false;
        }

        if (abilitiesSO.CheckBubbleShield == true)
        {
            bubbleShield.SetActive(true);
        }
    }

    private void OnEnable()
    {
        playerNuke = playerControls.Gameplay.Dash;
        playerNuke.Enable();
        playerNuke.performed += NukeAbility;
    }

    //Disables player controls when directed to
    private void OnDisable()
    {
        playerNuke.Disable();
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

    private void NukeAbility(InputAction.CallbackContext context)
    {
        if (abilitiesSO.CurrentNukeValue > 0)
        {
            if (readyToNuke == true)
            {
                abilitiesSO.CurrentNukeValue -= 1;
                NukeActivation();
            }
        }
    }

    private IEnumerator NukeActivation()
    {
        nukeBox.SetActive(true);
        yield return new WaitForSeconds(.25f);
        nukeBox.SetActive(false);
    }
}