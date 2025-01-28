using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private float dashSpeed = 3f;
    [SerializeField] private TrailRenderer myTrailRenderer;

    public float moveSpeed = 1f;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Knockback knockback;
    private float startingMoveSpeed;

    private bool isDashing = false;
    private bool canAttack = true; // Track if the player can attack
    [SerializeField] private float attackCooldown = 1f; // Cooldown duration

    protected override void Awake()
    {
        base.Awake();
        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
        weaponCollider.gameObject.SetActive(false);
    }

    private void Start()
    {
        playerControls.Movement.Dash.performed += _ => Dash();
        startingMoveSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void Update()
    {
        PlayerInput();
        CheckAttack();
    }

    private void FixedUpdate()
    {
        Move();
        PlayerDirection();
    }

    private void PlayerInput()
    {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        animator.SetFloat("moveX", movement.x);
        animator.SetFloat("moveY", movement.y);
    }

    private void Move()
    {
        if (knockback.GettingKnockedBack || PlayerHealth.Instance.isDead) { return; }

        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    public void PlayerDirection()
    {
        if (movement.x < 0)
        {
            spriteRenderer.flipX = true;
            weaponCollider.localPosition = new Vector3(0f, 0f, 0f);
            weaponCollider.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (movement.x > 0)
        {
            spriteRenderer.flipX = false;
            weaponCollider.localPosition = new Vector3(0f, 0f, 0f);
            weaponCollider.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    private void CheckAttack()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canAttack) // Check if player can attack
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private IEnumerator AttackRoutine()
    {
        canAttack = false; // Disable attacking during cooldown
        animator.SetBool("Attack", true);
        weaponCollider.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f); // Attack animation duration
        animator.SetBool("Attack", false);
        weaponCollider.gameObject.SetActive(false);

        yield return new WaitForSeconds(attackCooldown); // Cooldown duration
        canAttack = true; // Re-enable attacking
    }

    private void Dash()
    {
        if (isDashing || Energy.Instance.CurrentEnergy <= 0 || PlayerHealth.Instance.isDead)
        {
            return;
        }

        if (myTrailRenderer == null)
        {
            return;
        }

        Energy.Instance.UseStamina();
        isDashing = true;
        moveSpeed *= dashSpeed;
        myTrailRenderer.emitting = true;
        StartCoroutine(EndDashRoutine());
    }

    private IEnumerator EndDashRoutine()
    {
        float dashTime = 0.1f;
        float dashCD = 0.25f;
        yield return new WaitForSeconds(dashTime);
        myTrailRenderer.emitting = false;
        moveSpeed = startingMoveSpeed;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }
}
