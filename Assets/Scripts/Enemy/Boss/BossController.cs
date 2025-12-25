using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BossController : MonoBehaviour
{
    [Header("Referanslar")]
    public GameObject leftLaserObject; 
    public GameObject rightLaserObject;
    public BossAuraController auraScript;

    [Header("Zamanlama")]
    [SerializeField] private float laserCooldown = 4f; 
    [SerializeField] private float laserWarningTime = 1f; 
    [SerializeField] private float laserDuration = 2f; 

    [Header("Sesler")]
    [SerializeField] private AudioClip laserChargeClip;
    [SerializeField] private AudioClip laserLoopClip;

    private AudioSource audioSource;
    private bool isBattleActive = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        leftLaserObject.SetActive(false);
        rightLaserObject.SetActive(false);
    }

    public void StartBossBattle()
    {
        if (isBattleActive) return;

        isBattleActive = true;

        if (auraScript != null)
        {
            auraScript.StartAuraBattle();
        }
        else
        {
            Debug.LogWarning("BossController'da Aura Scripti atanmamış!");
        }

        StartCoroutine(LaserAttackRoutine());
    }

    IEnumerator LaserAttackRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(laserCooldown);

            if (laserChargeClip != null) audioSource.PlayOneShot(laserChargeClip);
            yield return new WaitForSeconds(laserWarningTime);

            leftLaserObject.SetActive(true);
            rightLaserObject.SetActive(true);

            if (laserLoopClip != null)
            {
                audioSource.clip = laserLoopClip;
                audioSource.loop = true;
                audioSource.Play();
            }
            
            yield return new WaitForSeconds(laserDuration);

            leftLaserObject.SetActive(false);
            rightLaserObject.SetActive(false);

            if (audioSource.isPlaying && audioSource.clip == laserLoopClip)
            {
                audioSource.Stop();
                audioSource.loop = false;
            }
        }
    }
}