using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float maxRight = 0.3f; // max x offset from start
    [SerializeField] private float maxUp = 0.2f;    // max y offset from start
    [SerializeField] private float moveSpeed = 0.01f; // movement speed (units per second)
    [SerializeField] private float reachThreshold = 0.01f; // distance to consider "reached"

    private Vector3 startPos;
    private Vector3 targetPos;

    void Start()
    {
        startPos = transform.position;
        PickNewTargetPosition();
    }

    void Update()
    {
        MoveTowardsTarget();
    }

    private void PickNewTargetPosition()
    {
        float x = Random.Range(startPos.x, startPos.x + maxRight);
        float y = Random.Range(startPos.y, startPos.y + maxUp);

        targetPos = new Vector3(x, y, startPos.z);
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < reachThreshold)
        {
            PickNewTargetPosition();
        }
    }
}
