using UnityEngine;

public class TargetScore : MonoBehaviour
{
    [Header("Scoring")]
    public int maxScore = 100; // Score for a perfect center hit
    public float maxRadius = 0f; // Max radius for scoring (beyond this = 0)

    [Tooltip("Optional: Target center override")]
    public Transform targetCenter;

    private void Start()
    {
        // If maxRadius isn't set manually, attempt to auto-calculate
        if (maxRadius <= 0f)
        {
            Collider col = GetComponent<Collider>();
            if (col != null)
            {
                maxRadius = col.bounds.extents.magnitude; // Approximate max radius
            }
            else
            {
                Debug.LogWarning("No collider found on target. Please set maxRadius manually.");
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Bullet")) return;

        Vector3 hitPoint = collision.GetContact(0).point;
        Vector3 center = targetCenter != null ? targetCenter.position : transform.position;

        float distance = Vector3.Distance(hitPoint, center);

        int score = 0;
        if (distance < maxRadius)
        {
            float normalized = 1f - (distance / maxRadius); // 1 = center, 0 = edge
            score = Mathf.RoundToInt(normalized * maxScore);
        }

        ScoreManager.Instance.AddScore(score);
    }
}
