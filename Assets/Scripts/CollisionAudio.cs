using UnityEngine;

public class CollisionAudio : MonoBehaviour
{
    public float minimumImpact = 0.1f;
    public float maximumImpact = 10f;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        float impact = collision.relativeVelocity.magnitude;
        if (impact < minimumImpact)
        return;
        float volume = Mathf.Clamp01(impact / maximumImpact);
        audioSource.volume = volume;
        audioSource.Play();
    }
}