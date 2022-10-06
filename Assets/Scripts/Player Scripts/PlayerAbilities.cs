using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerAbilities : MonoBehaviour
{
    [Header("Purchased Items")]
    public static bool hasMaxNukes;

    [Header("Abilities")]
    public GameObject bubbleShield;
    public GameObject nukeFX;
    public GameObject nukeBox;

    [Header("Player Input")]
    [SerializeField] public PlayerControls playerControls;
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private InputActionReference throwReference;

    public static bool playerHasScytheAbility;
    public Transform scythePosition;

    [SerializeField] private AbilitiesSO abilitiesSO;

    public GameObject scytheObject;

    GameObject target;

    [SerializeField] private AudioSource audioSource;
    public AudioClip nukeNoise;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        actionReference.action.Enable();
        throwReference.action.Enable();
    }

    private void OnDisable()
    {
        actionReference.action.Disable();
        throwReference.action.Disable();
    }

    void Start()
    {
        playerHasScytheAbility = abilitiesSO.CheckThrow;
        if (!(actionReference.action.interactions.Contains("Press") && actionReference.action.interactions.Contains("Hold")))
        {
            return;
        }

        if (!(throwReference.action.interactions.Contains("Press") && throwReference.action.interactions.Contains("Hold")))
        {
            return;
        }

        actionReference.action.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                NukeAbility();
            }
        };

        throwReference.action.performed += context =>
        {
            if (context.interaction is HoldInteraction)
            {
                Debug.Log("Throwing");
                ScytheThrow();
            }
        };
    }

    void Update()
    {

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

    private void NukeAbility()
    {
        if (abilitiesSO.CurrentNukeValue > 0)
        {
            Instantiate(nukeFX, transform.position, Quaternion.Euler(new Vector3(-90, 0, 0)));
            audioSource.PlayOneShot(nukeNoise, .2f);
            nukeBox.SetActive(true);
            abilitiesSO.CurrentNukeValue -= 1;
            StartCoroutine(NukeActivation());
        }
    }

    private void ScytheThrow()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (playerHasScytheAbility == true && scene.name != "TitleScreen" && scene.name != "HubShip")
        {
            Instantiate(scytheObject, scythePosition.position, Quaternion.identity);
        }
    }


    private IEnumerator NukeActivation()
    {
        yield return new WaitForSeconds(.25f);
        nukeBox.SetActive(false);
    }
}