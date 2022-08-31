using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Stats")]
    public float moveSpeed = 15f;

    [Header("Assignables")]
    [SerializeField] private Rigidbody2D playerRB;
    [SerializeField] private Transform playerOrientation;

    [Header("Player Input & Actions")]
    [SerializeField] public PlayerControls playerControls;
    private InputAction playerMove;
    private InputAction playerAttack;

    Vector2 movementInput = Vector2.zero;

    //Before start function assign our player controls to the input system
    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    //Enables player controls when directed to
    private void OnEnable()
    {
        playerMove = playerControls.Gameplay.Move;
        playerMove.Enable();

        playerAttack = playerControls.Gameplay.Attack;
        playerAttack.Enable();
        playerAttack.performed += Attack;
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
    }

    void Update()
    {
        movementInput = playerMove.ReadValue<Vector2>();
    }


    void FixedUpdate()
    {
        //Moving the player
        playerRB.velocity = new Vector2(movementInput.x * moveSpeed, movementInput.y * moveSpeed);
    }

    //Player's basic attack 
    private void Attack(InputAction.CallbackContext context)
    {
        Debug.Log("We attacked!");
    }
}
