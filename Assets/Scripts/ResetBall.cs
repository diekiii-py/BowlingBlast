using UnityEngine;

public class BallReset : MonoBehaviour
{
    public Rigidbody rb;
    public Transform respawnPoint;
    public float stopSpeed = 0.1f;
    private bool canReset;

    private void OnTriggerEnter(Collider other)
    {
        canReset = true;
    }
    
    private void FixedUpdate()
    {
        if (canReset && rb.linearVelocity.magnitude < stopSpeed)
        {
            Respawn();
        }
    }
    
    private void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = respawnPoint.position;
        rb.rotation = respawnPoint.rotation;
        canReset = false;
    }
}