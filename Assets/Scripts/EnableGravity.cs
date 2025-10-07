using UnityEngine;

public class EnableGravity : MonoBehaviour
{
    private OVRGrabbable grabbable;
    private Rigidbody rb;
    private bool wasGrabbedLastFrame;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        grabbable = GetComponent<OVRGrabbable>();
        rb = GetComponent<Rigidbody>();

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        wasGrabbedLastFrame = grabbable.isGrabbed;

        rb.isKinematic = true;
    }

    void Update()
    {
        bool isCurrentlyGrabbed = grabbable.isGrabbed;

        if (wasGrabbedLastFrame && !isCurrentlyGrabbed)
        {
            rb.isKinematic = false;
        }

        wasGrabbedLastFrame = isCurrentlyGrabbed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            ResetGun();
        }
    }

    private void ResetGun()
    {
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;

        if (grabbable.isGrabbed)
        {
            grabbable.grabbedBy.ForceRelease(grabbable);
        }
    }
}

