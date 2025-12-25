using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private float damage = 1f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            PlayerMovement movement = collision.GetComponent<PlayerMovement>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);

                if (playerHealth.currentHealth > 0)
                {
                    movement.Respawn();
                }
            }
        }
    }
}