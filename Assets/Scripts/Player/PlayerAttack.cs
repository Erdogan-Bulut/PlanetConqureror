using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("SFX")]
    [SerializeField] private AudioClip hitSound;
    private AudioSource audioSource;

    private Animator anim;
    private PlayerMovement playerMovement;
    private float cooldownTimer = Mathf.Infinity;
    private int attackIndex = 0;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && cooldownTimer > attackCooldown && playerMovement.canAttack())
        {
            Attack();
        }

        cooldownTimer += Time.deltaTime;
    }

    private void Attack()
    {
        anim.SetTrigger("attack" + attackIndex); 
        attackIndex = (attackIndex + 1) % 3; 
        cooldownTimer = 0;
    }

    private void DealDamageToAnyEnemy()
    {
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector3 castPos = boxCollider.bounds.center + (Vector3)direction * range * colliderDistance;
        Vector2 boxSize = new Vector2(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y);

        RaycastHit2D hit = Physics2D.BoxCast(castPos, boxSize, 0f, direction, 0f, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            Health health = hit.collider.GetComponent<Health>() ?? hit.collider.GetComponentInParent<Health>();
            
            if (health != null)
            {
                if(health.TakeDamage(damage))
                {
                    if (hitSound != null && audioSource != null)
                    {
                        audioSource.PlayOneShot(hitSound);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;
        Gizmos.color = Color.red;
        Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        Vector3 hitboxPos = boxCollider.bounds.center + (Vector3)direction * range * colliderDistance;
        Gizmos.DrawWireCube(hitboxPos, new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, 0.1f));
    }
}