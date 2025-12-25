using UnityEngine;

public class VerticalPatrol : EnemyPatrolBase
{
    [Header("Vertical Points")]
    [SerializeField] private Transform topEdge;
    [SerializeField] private Transform bottomEdge;

    private void Update()
    {
        if (movingForward)
        {
            if (enemy.position.y <= topEdge.position.y)
                Move(1);
            else
                HandleDirectionChange();
        }
        else
        {
            if (enemy.position.y >= bottomEdge.position.y)
                Move(-1);
            else
                HandleDirectionChange();
        }
    }

    private void Move(int direction)
    {
        idleTimer = 0;
        if (anim != null) anim.SetBool("moving", true);

        enemy.position = new Vector3(enemy.position.x,
            enemy.position.y + Time.deltaTime * direction * speed, enemy.position.z);
    }
}