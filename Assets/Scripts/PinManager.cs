using UnityEngine;

public class PinManager : MonoBehaviour
{
    public Pin[] pins;
    private Vector3[] startPositions;
    private Quaternion[] startRotations;
   
    private void Start()
    {
        startPositions = new Vector3[pins.Length];
        startRotations = new Quaternion[pins.Length];
        f
        or (int i = 0; i < pins.Length; i++)
        {
            startPositions[i] = pins[i].transform.position;
            startRotations[i] = pins[i].transform.rotation;
        }
    }
    private void Update()
    {
        CheckPins();
    }
    
    private void CheckPins()
    {
        foreach (Pin pin in pins)
        {
            if (!pin.IsFallen())
            return;
        }
        ResetPins();
    }
    
    private void ResetPins()
    {
        for (int i = 0; i < pins.Length; i++)
        {
            Rigidbody rb = pins[i].GetComponent<Rigidbody>();
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.position = startPositions[i];
            rb.rotation = startRotations[i];
        }
    }
}