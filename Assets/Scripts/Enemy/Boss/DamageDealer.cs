using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private int damageAmount = 10;
    [SerializeField] private bool isContinuousDamage = false;
    [SerializeField] private float damageInterval = 0.5f;
    
    private float nextDamageTime;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyDamage(other);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isContinuousDamage && other.CompareTag("Player"))
        {
            if (Time.time >= nextDamageTime)
            {
                ApplyDamage(other);
                nextDamageTime = Time.time + damageInterval;
            }
        }
    }

    private void ApplyDamage(Collider2D player)
    {
        player.GetComponent<Health>().TakeDamage(damageAmount);
    }
}