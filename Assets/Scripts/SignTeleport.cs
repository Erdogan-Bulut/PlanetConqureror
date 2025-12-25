using UnityEngine;
public class SignTeleport : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Transform player;
    [SerializeField] private CameraController2 cam;
    [SerializeField] private Transform nextLevel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            player.position = nextLevel.position;
            cam.SnapToPlayer();
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
}
