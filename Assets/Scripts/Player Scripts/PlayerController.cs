using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 15f;
    [SerializeField] public int currentHealth { get; private set; }
    public static int maxHealth = 5;
    public int maxHealthIndicator;

    [Header("Invincibility Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numOfFlashes;
    private SpriteRenderer playerSpriteRend;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private ScytheAttack scytheAttack;
    [SerializeField] private Animator anim;
    [SerializeField] private AudioSource audioSource;
    public AudioClip hurtNoise;
    public AudioClip scytheAttackNoise;

    [Header("Player Input & Actions")]
    [SerializeField] public PlayerControls playerControls;
    private InputAction playerMove;
    private InputAction playerAttack;
    private Vector2 movementInput = Vector2.zero;

    public static event Action OnPlayerDeath;
    private bool attackResetted;
    Vector2 lookDirection = new Vector2(1,0); 

    //Before start function assign our player controls to the input system
    private void Awake()
    {
        attackResetted = true;
        playerControls = new PlayerControls();
    }

    //Enables player controls when directed to
    private void OnEnable()
    {
        playerMove = playerControls.Gameplay.Move;
        playerMove.Enable();

        playerAttack = playerControls.Gameplay.Attack;
        playerAttack.Enable();
        playerAttack.performed += ScytheAttack;
    }

    //Disables player controls when directed to
    private void OnDisable()
    {
        playerMove.Disable();
        playerAttack.Disable();
    }

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        playerSpriteRend = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();

        currentHealth = maxHealth;
    }

    void Update()
    {
        maxHealthIndicator = maxHealth;
        movementInput = playerMove.ReadValue<Vector2>();
                
        if(!Mathf.Approximately(movementInput.x, 0.0f) || !Mathf.Approximately(movementInput.y, 0.0f))
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

    //Player's basic attack 
    private void ScytheAttack(InputAction.CallbackContext context)
    {
        Debug.Log("Attack button pressed");

        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "TitleScreen" && scene.name != "HubShip")
        {
            //Cooldown for attacking
            if (attackResetted == true)
            {
                audioSource.PlayOneShot(scytheAttackNoise);
                if (playerSpriteRend.flipX == true)
                {
                    scytheAttack.AttackLeft();
                    StartCoroutine(ResetAttack());
                }

                if (playerSpriteRend.flipX == false)
                {
                    scytheAttack.AttackRight();
                    StartCoroutine(ResetAttack());
                }
            }
        }

    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Health Left: " + currentHealth);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        audioSource.PlayOneShot(hurtNoise);
        if (currentHealth > 0)
        {
            StartCoroutine(Invulnerability());
        }

        if (currentHealth <= 0)
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
        attackResetted = false;
        yield return new WaitForSeconds(.5f);
        attackResetted = true;
        scytheAttack.StopAttack();
    }
}
