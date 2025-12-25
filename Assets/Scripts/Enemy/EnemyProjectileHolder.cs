using UnityEngine;

public class EnemyProjectileHolder : MonoBehaviour
{
    [SerializeField] private Transform enemy;

    private void Update()
    {
        float direction = Mathf.Sign(enemy.localScale.x);

        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction * -1,
        transform.localScale.y, transform.localScale.z);
    }
}
