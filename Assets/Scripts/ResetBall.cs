using System.Collections;
using UnityEngine;

public class BallReset : MonoBehaviour
{
    public Rigidbody rb;
    public Transform respawnPoint;
    public GameObject[] resetAreas;
    public GameObject[] ballOnlyResetAreas;
    public float stopSpeed = 0.1f;
    public float ballOnlyResetDelay = 0.5f;
    public bool HasEnteredRespawnTrigger { get; private set; }
    public bool HasHitPins { get; private set; }
    public bool HasStoppedMoving => rb.linearVelocity.magnitude < stopSpeed;
    private bool canDetectResetSurfaces;
    private bool ballOnlyResetPending;

    private IEnumerator Start()
    {
        yield return new WaitForFixedUpdate();
        canDetectResetSurfaces = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDetectResetSurfaces)
            return;

        if (IsBallOnlyResetArea(other.gameObject))
        {
            StartBallOnlyReset();
            return;
        }

        if (IsResetArea(other.gameObject))
            HasEnteredRespawnTrigger = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canDetectResetSurfaces && IsBallOnlyResetArea(collision.collider.gameObject))
        {
            StartBallOnlyReset();
            return;
        }

        if (canDetectResetSurfaces && IsResetArea(collision.collider.gameObject))
            HasEnteredRespawnTrigger = true;

        if (collision.collider.GetComponentInParent<Pin>() != null)
            HasHitPins = true;
    }

    private bool IsResetArea(GameObject contactedObject)
    {
        if (resetAreas == null)
            return false;

        foreach (GameObject resetArea in resetAreas)
        {
            if (resetArea == null)
                continue;

            if (contactedObject == resetArea || contactedObject.transform.IsChildOf(resetArea.transform))
                return true;
        }

        return false;
    }

    private bool IsBallOnlyResetArea(GameObject contactedObject)
    {
        if (ballOnlyResetAreas == null)
            return false;

        foreach (GameObject resetArea in ballOnlyResetAreas)
        {
            if (resetArea != null &&
                (contactedObject == resetArea || contactedObject.transform.IsChildOf(resetArea.transform)))
                return true;
        }

        return false;
    }

    private void StartBallOnlyReset()
    {
        if (ballOnlyResetPending)
            return;

        StartCoroutine(BallOnlyResetSequence());
    }

    private IEnumerator BallOnlyResetSequence()
    {
        ballOnlyResetPending = true;
        yield return new WaitForSeconds(ballOnlyResetDelay);
        ResetNow();
        ClearRespawnTrigger();
        ballOnlyResetPending = false;
    }
    
    public void ResetNow()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.position = respawnPoint.position;
        rb.rotation = respawnPoint.rotation;
    }

    public void ClearRespawnTrigger()
    {
        HasEnteredRespawnTrigger = false;
        HasHitPins = false;
    }
}