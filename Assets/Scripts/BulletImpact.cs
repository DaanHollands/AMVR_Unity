using UnityEngine;

public class BulletImpact : MonoBehaviour
{
    [Header("Impact Settings")]
    [SerializeField] private AudioClip impactSound; // The impact sound to play
    [SerializeField] private float destroyTimer = 2f; // Time before destroying the bullet after impact
    private AudioSource audioSource;

    void Start()
    {
        // Get or add an AudioSource component
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the impact sound is set and play it
        if (impactSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(impactSound);
        }

        // Optionally destroy the bullet after impact
        Destroy(gameObject, destroyTimer);
    }
}
