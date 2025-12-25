using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _speed = 5f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump Settings")]
    [SerializeField] private float _maxJumpForce = 6.3f;
    [SerializeField] private float _minJumpForce = 4.5f;

    [Header("Respawn Settings")]
    private Vector3 respawnPoint;

    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;

    private float horizontalInput;
    private bool jumpRequest;
    private bool jumpReleaseRequest;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
    }

    void Start()
    {
        respawnPoint = transform.position;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && Mathf.Abs(body.linearVelocity.y) < 0.01f)
        {
            UpdateRespawnPoint(transform.position);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            jumpRequest = true;

        if (Input.GetKeyUp(KeyCode.Space))
            jumpReleaseRequest = true;

        HandleFlip();

        bool grounded = IsGrounded();
        bool isMoving = Mathf.Abs(horizontalInput) > 0.01f;

        anim.SetBool("isRunning", isMoving && grounded && !IsAttacking());
        anim.SetBool("isGrounded", grounded);
        anim.SetFloat("verticalVelocity", body.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (IsAttacking() && IsGrounded())
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
        }
        else
        {
            body.linearVelocity = new Vector2(horizontalInput * _speed, body.linearVelocity.y);
        }

        ApplyJump();
    }

    public void Respawn()
    {
        transform.position = respawnPoint;
        body.linearVelocity = Vector2.zero;
    }

    public void UpdateRespawnPoint(Vector3 newPoint)
    {
        respawnPoint = newPoint;
    }

    private bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.2f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool IsAttacking()
    {
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);
        return state.IsName("Attack1") || state.IsName("Attack2") || state.IsName("Attack3");
    }

    public bool canAttack()
    {
        bool isWalkingOnGround = Mathf.Abs(horizontalInput) > 0.01f && IsGrounded();
        return !isWalkingOnGround;
    }

    private void ApplyJump()
    {
        if (jumpRequest)
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, _maxJumpForce);
            anim.SetTrigger("jump");
            jumpRequest = false;
        }

        if (jumpReleaseRequest)
        {
            if (body.linearVelocity.y > _minJumpForce)
                body.linearVelocity = new Vector2(body.linearVelocity.x, _minJumpForce);
            jumpReleaseRequest = false;
        }
    }

    private void HandleFlip()
    {
        if (IsAttacking() && IsGrounded()) return;

        if (horizontalInput > 0.01f)
            transform.localScale = new Vector3(2, 2, 2);
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-2, 2, 2);
    }
}