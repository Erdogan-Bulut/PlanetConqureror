using System.Collections;
using UnityEngine;

public class HorizontalPatrol : EnemyPatrolBase
{
    [Header("Horizontal Points")]
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;

    [Header("Smoothing")]
    [SerializeField] private float turnDuration = 0.5f;
    
    private bool isTurning = false;
    private Coroutine turnRoutine;

    private void Update()
    {
        if (isTurning) return;

        if (movingForward)
        {
            if (enemy.position.x <= rightEdge.position.x)
                Move(1);
            else
                HandleDirectionChange();
        }
        else
        {
            if (enemy.position.x >= leftEdge.position.x)
                Move(-1);
            else
                HandleDirectionChange();
        }
    }

    public override void FaceDirection(float _direction)
    {
        if (isTurning) return;
        
        bool turnRight = _direction > 0;
        bool currentlyFacingRight = movingForward;

        if (turnRight != currentlyFacingRight)
        {

            if (turnRoutine != null) StopCoroutine(turnRoutine);

            turnRoutine = StartCoroutine(SmoothTurnRoutine(turnRight ? 1 : -1));
        }
    }

    private IEnumerator SmoothTurnRoutine(int direction)
    {
        isTurning = true;
        movingForward = (direction > 0);
        if (anim != null) anim.SetBool("moving", false);

        Vector3 startScale = enemy.localScale;
        Vector3 targetScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);

        float elapsedTime = 0;

        while (elapsedTime < turnDuration)
        {
            enemy.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / turnDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.localScale = targetScale;
        
        isTurning = false;
        turnRoutine = null;
    }

    private void Move(int direction)
    {
        idleTimer = 0;
        if (anim != null) anim.SetBool("moving", true);

        if (!isTurning) 
        {
            enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * direction, initScale.y, initScale.z);
        }

        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * direction * speed,
                                   enemy.position.y, enemy.position.z);
    }
}