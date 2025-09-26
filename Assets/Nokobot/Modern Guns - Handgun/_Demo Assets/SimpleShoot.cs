using UnityEngine;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 200f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    [Header("Audio")]
    [Tooltip("Audio source that plays when gun is fired")][SerializeField] private AudioSource gunFireSound;

    private OVRGrabbable grabbable;

    void Start()
    {
        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        grabbable = GetComponent<OVRGrabbable>();
    }

    void Update()
    {
        if (grabbable != null && grabbable.isGrabbed)
        {
            OVRGrabber grabbingHand = grabbable.grabbedBy;

            if (grabbingHand != null)
            {
                string handName = grabbingHand.gameObject.name;

                OVRInput.Controller controller;

                if (handName.Contains("Left"))
                {
                    controller = OVRInput.Controller.LTouch;
                }
                else if (handName.Contains("Right"))
                {
                    controller = OVRInput.Controller.RTouch;
                }
                else
                {
                    controller = OVRInput.Controller.Active;
                }

                OVRInput.Button triggerButton = controller == OVRInput.Controller.LTouch
                    ? OVRInput.Button.PrimaryIndexTrigger
                    : OVRInput.Button.SecondaryIndexTrigger;

                if (OVRInput.GetDown(triggerButton))
                {
                    gunAnimator.SetTrigger("Fire");
                }
            }
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        if (gunFireSound != null && gunFireSound.clip != null)
        {
            gunFireSound.Play();
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }

        // Create a bullet, change it's layer and add force on it in direction of the barrel
        Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
