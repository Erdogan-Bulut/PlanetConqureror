using UnityEngine;

public class CameraController2 : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, -10f);
    
    [Header("Smooth Settings")]
    [SerializeField] private float smoothTime = 0.25f;
    [SerializeField] private float aheadDistance = 2f;
    
    private Vector3 velocity = Vector3.zero;
    private float lookAhead;

    void LateUpdate()
    {
        float targetLookAhead = (player.localScale.x > 0) ? aheadDistance : -aheadDistance;
        lookAhead = Mathf.Lerp(lookAhead, targetLookAhead, Time.deltaTime * 5f);

        Vector3 targetPosition = new Vector3(player.position.x + lookAhead, player.position.y + offset.y, offset.z);

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    public void SnapToPlayer()
    {
        lookAhead = (player.localScale.x > 0) ? aheadDistance : -aheadDistance;
        transform.position = new Vector3(player.position.x + lookAhead, player.position.y + offset.y, offset.z);
        velocity = Vector3.zero;
    }
}