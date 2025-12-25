using UnityEngine;

public class TeleportAndAwakeBoss : MonoBehaviour
{
    [Header("Ayarlar")]
    public Transform teleportTarget;
    public BossController bossScript;
    
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            hasTriggered = true;

            other.transform.position = teleportTarget.position;

            if (bossScript != null)
            {
                bossScript.StartBossBattle();
            }
            else
            {
                Debug.LogError("Tabelaya BossController scripti atanmamış!");
            }
        }
    }
}