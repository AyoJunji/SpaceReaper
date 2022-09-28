using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 15f;
    public float dashForce = 3f;

    [Header("Invincibility Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numOfFlashes;
    private SpriteRenderer playerSpriteRend;

    [Header("Assignables")]
    [SerializeField] private HealthSO healthSO;
    [SerializeField] private AbilitiesSO abilitiesSO;
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private ScytheAttack scytheAttack;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;
    public AudioClip hurtNoise;
    public AudioClip scytheAttackNoise;

    [Header("Player Input & Actions")]
    [SerializeField] public PlayerControls playerControls;
    [SerializeField] private InputActionReference actionReference;
    [SerializeField] private InputActionReference dashReference;
    private InputAction playerMove;
    private InputAction playerAttack;
    private InputAction playerDash;
    private Vector2 movementInput = Vector2.zero;
    private Vector2 moveDirection;

    public static event Action OnPlayerDeath;
    private bool attackReset;
    Vector2 lookDirection = new Vector2(1, 0);

    //Before start function assign our player controls to the input system
    private void Awake()
    {
        attackReset = true;
        playerControls = new PlayerControls();
    }

    //Enables player controls when directed to
    private void OnEnable()
    {
        playerMove = playerControls.Gameplay.Move;
        playerMove.Enable();

        dashReference.action.Enable();
        actionReference.action.Enable();
    }

    //Disables player controls when directed to
    private void OnDisable()
    {
        playerMove.Disable();

        dashReference.action.Disable();
        actionReference.action.Disable();
    }

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerSpriteRend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        if (!(actionReference.action.interactions.Contains("Press") && actionReference.action.interactions.Contains("Hold")))
        {
            return;
        }

        if (!(dashReference.action.interactions.Contains("Press") && dashReference.action.interactions.Contains("Hold")))
        {
            return;
        }

        actionReference.action.performed += context =>
        {
            if (context.interaction is PressInteraction)
            {
                Debug.Log("Scythe Attack");
                ScytheAttack();
            }
        };

        dashReference.action.performed += context =>
        {
            if (context.interaction is PressInteraction)
            {

                Dash();
            }
        };
    }

    void Update()
    {
        movementInput = playerMove.ReadValue<Vector2>();
        moveDirection = new Vector2(movementInput.x, movementInput.y);

        if (!Mathf.Approximately(movementInput.x, 0.0f) || !Mathf.Approximately(movementInput.y, 0.0f))
        {
            lookDirection.Set(movementInput.x, movementInput.y);
            lookDirection.Normalize();
        }

        anim.SetFloat("Move X", lookDirection.x);
        anim.SetFloat("Move Y", lookDirection.y);
        anim.SetFloat("Speed", movementInput.magnitude);
    }


    void FixedUpdate()
    {
        //Moving the player
        playerRB.velocity = new Vector2(movementInput.x * moveSpeed, movementInput.y * moveSpeed);

    }

    private void Dash()
    {
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Dash input");
        if (abilitiesSO.CheckDash == true && scene.name != "TitleScreen" && scene.name != "HubShip")
        {
            Debug.Log("Dash input went through");
            playerRB.AddForce(moveDirection * dashForce, ForceMode2D.Impulse);
        }
    }


    //Player's basic attack 
    private void ScytheAttack()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "TitleScreen" && scene.name != "HubShip")
        {
            //Cooldown for attacking
            if (attackReset == true)
            {
                anim.SetTrigger("Attack");
                audioSource.PlayOneShot(scytheAttackNoise, .2f);
                if (movementInput.x < 0)
                {
                    scytheAttack.AttackLeft();
                    StartCoroutine(ResetAttack());
                }

                if (movementInput.x > 0)
                {
                    scytheAttack.AttackRight();
                    StartCoroutine(ResetAttack());
                }
            }
        }

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Health Left: " + healthSO.CurrentHealthValue);
        healthSO.CurrentHealthValue = Mathf.Clamp(healthSO.CurrentHealthValue - damage, 0, healthSO.MaxHealthValue);

        audioSource.PlayOneShot(hurtNoise, .3f);

        if (healthSO.CurrentHealthValue > 0)
        {
            StartCoroutine(Invulnerability());
        }

        if (healthSO.CurrentHealthValue <= 0)
        {
            //Disable controls
            OnDisable();

            //Display death menu
            OnPlayerDeath?.Invoke();
        }
    }

    //Invulnerable frames and number of flashes function
    private IEnumerator Invulnerability()
    {
        Physics2D.IgnoreLayerCollision(6, 7, true);
        for (int i = 0; i < numOfFlashes; i++)
        {
            playerSpriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numOfFlashes * 2));
            playerSpriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(6, 7, false);

    }

    private IEnumerator ResetAttack()
    {
        attackReset = false;
        yield return new WaitForSeconds(.5f);
        attackReset = true;
        scytheAttack.StopAttack();
    }
}
