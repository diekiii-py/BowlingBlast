using System.Collections;
using UnityEngine;

public class PinManager : MonoBehaviour
{
    public Pin[] pins;
    public UnityEngine.Animator resetAnimator;
    public BallReset ballReset;
    public BowlingScoreManager scoreManager;
    public float settleTime = 0.5f;
    public float maximumPinSpeed = 0.1f;
    public float maximumPinAngularSpeed = 0.1f;
    public float resetDownDuration = 0.42f;
    public bool resetPinsWithoutBall = true;

    private Vector3[] startPositions;
    private Quaternion[] startRotations;
    private BallReset[] ballResets;
    private float settledTime;
    private bool isResetting;
   
    private void Start()
    {
        if (resetAnimator == null)
        {
            UnityEngine.Animator[] animators = FindObjectsByType<UnityEngine.Animator>(FindObjectsSortMode.None);
            foreach (UnityEngine.Animator animator in animators)
            {
                if (animator.runtimeAnimatorController != null &&
                    animator.runtimeAnimatorController.name == "Reset")
                {
                    resetAnimator = animator;
                    break;
                }
            }

            GameObject resetObject = GameObject.Find("Reset");
            if (resetAnimator == null && resetObject != null)
                resetAnimator = resetObject.GetComponent<UnityEngine.Animator>();
        }

        ballResets = FindObjectsByType<BallReset>(FindObjectsSortMode.None);
        if (ballReset == null && ballResets.Length > 0)
            ballReset = ballResets[0];

        startPositions = new Vector3[pins.Length];
        startRotations = new Quaternion[pins.Length];
        for (int i = 0; i < pins.Length; i++)
        {
            startPositions[i] = pins[i].transform.position;
            startRotations[i] = pins[i].transform.rotation;
        }
    }
    private void Update()
    {
        if (isResetting)
            return;

        SelectTriggeredBall();

        if (ShouldReset())
        {
            settledTime += Time.deltaTime;
            if (settledTime >= settleTime)
                StartCoroutine(ResetSequence());
        }
        else
        {
            settledTime = 0f;
        }
    }

    private void SelectTriggeredBall()
    {
        if (ballResets == null)
            return;

        foreach (BallReset candidate in ballResets)
        {
            if (candidate.HasEnteredRespawnTrigger)
            {
                ballReset = candidate;
                return;
            }
        }
    }

    private bool ShouldReset()
    {
        bool ballEnteredTrigger = ballReset != null && ballReset.HasEnteredRespawnTrigger;
        if (!ballEnteredTrigger)
            return resetPinsWithoutBall && AnyPinIsFallen() && PinsAreSettled();

        if (ballReset.HasStoppedMoving)
            return true;

        if (!ballReset.HasHitPins)
            return false;

        return PinsAreSettled();
    }

    private bool AnyPinIsFallen()
    {
        foreach (Pin pin in pins)
        {
            if (pin.IsFallen())
                return true;
        }

        return false;
    }

    private bool PinsAreSettled()
    {
        foreach (Pin pin in pins)
        {
            if (!pin.IsFallen())
                continue;

            Rigidbody rb = pin.GetComponent<Rigidbody>();
            if (rb != null && (rb.linearVelocity.magnitude > maximumPinSpeed ||
                               rb.angularVelocity.magnitude > maximumPinAngularSpeed))
                return false;
        }

        return true;
    }

    private IEnumerator ResetSequence()
    {
        isResetting = true;
        settledTime = 0f;

        if (scoreManager != null)
            scoreManager.RecordRoll(CaptureFallenPins());

        if (resetAnimator != null)
            resetAnimator.Play("Reset Down", 0, 0f);

        yield return new WaitForSeconds(resetDownDuration);

        if (ballReset != null && ballReset.HasEnteredRespawnTrigger)
            ballReset.ResetNow();

        ResetPins();

        if (ballReset != null && ballReset.HasEnteredRespawnTrigger)
            ballReset.ClearRespawnTrigger();

        if (resetAnimator != null)
            resetAnimator.Play("Reset Up", 0, 0f);

        isResetting = false;
    }

    private bool[] CaptureFallenPins()
    {
        bool[] fallenPins = new bool[pins.Length];
        for (int i = 0; i < pins.Length; i++)
            fallenPins[i] = pins[i].IsFallen();

        return fallenPins;
    }

    private void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Rigidbody rb = pins[i].GetComponent<Rigidbody>();
            if (rb == null)
                continue;

            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = startPositions[i];
            rb.rotation = startRotations[i];
        }
    }
}