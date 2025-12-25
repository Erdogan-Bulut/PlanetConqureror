using UnityEngine;

public abstract class EnemyPatrolBase : MonoBehaviour
{
    [Header("Enemy & Animator")]
    [SerializeField] protected Transform enemy;
    [SerializeField] protected Animator anim;

    [Header("Movement Parameters")]
    [SerializeField] protected float speed;
    [SerializeField] protected float idleDuration;

    protected Vector3 initScale;
    protected float idleTimer;
    protected bool movingForward = true;

    protected virtual void Awake()
    {
        initScale = enemy.localScale;
    }

    protected virtual void OnDisable()
    {
        if (anim != null) anim.SetBool("moving", false);
    }

    protected void HandleDirectionChange()
    {
        if (anim != null) anim.SetBool("moving", false);
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration)
        {
            movingForward = !movingForward;
            idleTimer = 0;
        }
    }

    public virtual void FaceDirection(float _direction)
    {

    }
}