using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 15f;
    [SerializeField] public int currentHealth { get; private set; }
    private int maxHealth = 5;

    [Header("Invincibility Frames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numOfFlashes;
    private SpriteRenderer playerSpriteRend;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform playerOrientation;
    [SerializeField] private ScytheAttack scytheAttack;

    [Header("Player Input & Actions")]
    [SerializeField] public PlayerControls playerControls;
    private InputAction playerMove;
    private InputAction playerAttack;
    private Vector2 movementInput = Vector2.zero;

    public static event Action OnPlayerDeath;
    private bool attackResetted;

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

        currentHealth = maxHealth;
    }

    void Update()
    {
        movementInput = playerMove.ReadValue<Vector2>();

        //Set direction of sprite to movement direction
        if (movementInput.x < 0)
        {
            playerSpriteRend.flipX = true;
        }
        else if (movementInput.x > 0)
        {
            playerSpriteRend.flipX = false;

        }
    }


    void FixedUpdate()
    {
        //Moving the player
        playerRB.velocity = new Vector2(movementInput.x * moveSpeed, movementInput.y * moveSpeed);

    }

    //Player's basic attack 
    private void ScytheAttack(InputAction.CallbackContext context)
    {
        Debug.Log("You attacked!");
        //Cooldown for attacking
        if (attackResetted == true)
        {
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

    public void TakeDamage(int damage)
    {
        Debug.Log("Health Left: " + currentHealth);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

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
        Physics2D.IgnoreLayerCollision(6, 8, true);
        for (int i = 0; i < numOfFlashes; i++)
        {
            playerSpriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numOfFlashes * 2));
            playerSpriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numOfFlashes * 2));
        }

        Physics2D.IgnoreLayerCollision(6, 7, false);
        Physics2D.IgnoreLayerCollision(6, 8, false);
    }

    private IEnumerator ResetAttack()
    {
        attackResetted = false;
        yield return new WaitForSeconds(.5f);
        attackResetted = true;
        scytheAttack.StopAttack();
    }
}
