using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 6f;
    public float jumpHeight = 2f;

    [Header("Ground Check")]
    public Transform groundCheck, headCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask, deathMask, platformMask;
    public float jumpCoolDownTimer = 0.3f;

    private Animator anim;
    private Rigidbody rb;
    private InputSystem_Actions inputs;
    private Vector2 moveInput;
    private Vector3 velocity;

    private bool isGrounded, isOnDeathLayer, gameFail;
    private bool canDoubleJump;
    private float timer;
    float gravityFactor = 3f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ; // for 2D side movement

        anim = GetComponentInChildren<Animator>();
        inputs = new InputSystem_Actions();

        inputs.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        inputs.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        inputs.Player.Jump.performed += ctx => Jump();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }
    private void OnDisable() => inputs.Disable();

    private void Update()
    {
        if (gameFail)
            return;

        GroundCheck();
        HeadChecker();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        if (gameFail)
            return;

        ApplyGravity();
    }

    void LateUpdate()
    {
        if (isGrounded)
            CheckIfOnPlatform();
    }
    void CheckIfOnPlatform()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.5f, platformMask))
        {
            transform.SetParent(hit.transform.parent);
        }
        else
            transform.SetParent(GameManager.Instance.activeLevelManager.transform);
    }
    
    private void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isOnDeathLayer = Physics.CheckSphere(groundCheck.position, groundDistance, deathMask);

        if (isOnDeathLayer)
        {
            PlayerHealth.PlayerHit?.Invoke(10000);
            isOnDeathLayer = false;
            gameFail = true;
            return;
        }

        if (isGrounded)
        {
            timer = 0;
            canDoubleJump = true;
            if (velocity.y < 0)
                velocity.y = -2f; // small downward force to keep grounded
        }
        else
        {
            timer += Time.deltaTime;
        }
    }

    private bool HeadChecker()
    {
        if (Physics.CheckSphere(headCheck.position, groundDistance))
        {
            gravityFactor = 6f;
            return true;
        }
        gravityFactor = 3f;
        return false;
    }

    private void ApplyGravity()
    {
        // Manually apply gravity if not grounded
        if (!isGrounded)
        {
            rb.AddForce(Physics.gravity * gravityFactor, ForceMode.Acceleration);
        }
    }

    private void HandleMovement()
    {
        Vector3 move = new Vector3(moveInput.x * moveSpeed, rb.linearVelocity.y, 0); // keep current vertical velocity

        rb.linearVelocity = move;
        velocity.x = rb.linearVelocity.x;
        velocity.y = rb.linearVelocity.y;

        RotatePlayer(new Vector3(moveInput.x, 0, 0));

        if (anim)
            Utility.SetAnimation(anim, Utility.AnimationStates.MoveX, Mathf.Abs(velocity.x));
    }

    private void RotatePlayer(Vector3 move)
    {
        if (move.x > 0.1f)
            transform.rotation = Quaternion.Euler(0, 90f, 0);
        else if (move.x < -0.1f)
            transform.rotation = Quaternion.Euler(0, -90f, 0);
    }

    private void Jump()
    {
        if (isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -Physics.gravity.y);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, velocity.y, rb.linearVelocity.z);
            canDoubleJump = true;
            AudioManager.Instance.PlayJumpSound();
        }
        else if (canDoubleJump)
        {
            if (timer >= jumpCoolDownTimer)
                return;

            velocity.y = Mathf.Sqrt(jumpHeight * -Physics.gravity.y);
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, velocity.y, rb.linearVelocity.z);
            canDoubleJump = false;
            AudioManager.Instance.PlayJumpSound();
        }

        if (anim)
            anim.SetLayerWeight(1, 1);
    }
}
