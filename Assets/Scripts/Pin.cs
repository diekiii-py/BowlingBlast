using UnityEngine;

public class Pin : MonoBehaviour
{
    public float fallenAngle = 45f;
    public bool IsFallen()
    {
        float angle = Vector3.Angle(transform.up, Vector3.up);
        return angle > fallenAngle;
    }
}