using System.Collections;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float startingHealth;
    public float currentHealth { get; private set; }
    private Animator anim;
    private bool dead;
    private bool invulnerable;

    [Header("iFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;
    private SpriteRenderer spriteRend;

    public System.Action OnDamaged;

    [Header("SFX")]
    [SerializeField] private AudioClip hurtSound;
    private AudioSource audioSource;

    [Header("Death Components")]
    [SerializeField] private Behaviour[] components;

    [Header("End Game Settings")]
    [SerializeField] private bool isFinalBoss;

    private void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    public bool TakeDamage(float _damage)
    {
        if (invulnerable || dead) return false;

        currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
        OnDamaged?.Invoke();

        if (currentHealth > 0)
        {
            if (gameObject.CompareTag("Player") && hurtSound != null)
                audioSource.PlayOneShot(hurtSound);

            anim.SetTrigger("hurt");
            StartCoroutine(Invunerability());
        }
        else if (!dead) Die();

        return true;
    }

    private void Die()
    {
        dead = true;
        anim.SetTrigger("die");
        foreach (Behaviour component in components) if (component != null) component.enabled = false;

        if (gameObject.CompareTag("Player"))
        {
            UIManager ui = Object.FindAnyObjectByType<UIManager>();
            if (ui != null) ui.GameOver();
        }
        else if (isFinalBoss)
        {
            UIManager ui = Object.FindAnyObjectByType<UIManager>();
            if (ui != null) ui.WinGame();
        }
    }

    private IEnumerator Invunerability()
    {
        invulnerable = true;
        Physics2D.IgnoreLayerCollision(10, 11, true);
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration / (numberOfFlashes * 2));
        }
        Physics2D.IgnoreLayerCollision(10, 11, false);
        invulnerable = false;
    }

    private void Deactivate()
    {
        if (!gameObject.CompareTag("Player")) gameObject.SetActive(false);
    }
}