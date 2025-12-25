using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(AudioSource))]
public class BossAuraController : MonoBehaviour
{
    [Header("Zamanlama")]
    [SerializeField] private float minCooldown = 3f;
    [SerializeField] private float maxCooldown = 7f;
    [SerializeField] private float activeDuration = 2.5f;

    [Header("Görsel & Ses")]
    [SerializeField] private int blinkCount = 4;
    [SerializeField] private float blinkSpeed = 0.15f;
    [SerializeField] private Color warningColor = new Color(1f, 0.5f, 0.5f, 0.4f);
    [SerializeField] private AudioClip warningClip;
    [SerializeField] private AudioClip activeClip;

    private SpriteRenderer spriteRenderer;
    private Collider2D damageCollider;
    private AudioSource audioSource;
    private Color activeColor;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        damageCollider = GetComponent<Collider2D>();
        audioSource = GetComponent<AudioSource>();
        activeColor = spriteRenderer.color;
    }

    private void Start()
    {
        ResetAura();
    }

    public void StartAuraBattle()
    {
        ResetAura();
        StartCoroutine(AuraAttackLoop());
    }

    private void ResetAura()
    {
        spriteRenderer.enabled = false;
        damageCollider.enabled = false;
        spriteRenderer.color = activeColor;
    }

    IEnumerator AuraAttackLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minCooldown, maxCooldown);
            yield return new WaitForSeconds(waitTime);

            if (warningClip != null) audioSource.PlayOneShot(warningClip);

            for (int i = 0; i < blinkCount; i++)
            {
                spriteRenderer.enabled = true;
                spriteRenderer.color = warningColor;
                yield return new WaitForSeconds(blinkSpeed);
                spriteRenderer.enabled = false;
                yield return new WaitForSeconds(blinkSpeed);
            }

            spriteRenderer.enabled = true;
            spriteRenderer.color = activeColor;
            damageCollider.enabled = true;
            if (activeClip != null) audioSource.PlayOneShot(activeClip);

            yield return new WaitForSeconds(activeDuration);

            ResetAura();
        }
    }
}