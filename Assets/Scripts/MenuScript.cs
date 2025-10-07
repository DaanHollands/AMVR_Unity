using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    [Header("Menu Canvas")]
    public GameObject menuCanvas; // World space canvas
    public Transform playerCamera; // Usually CenterEyeAnchor

    [Header("UI Buttons")]
    public Button resetScoreButton;
    public Button resetAllButton;
    public Button resetItemsButton;

    [Header("Laser Pointer")]
    public LineRenderer pointer;

    [Header("Reset Targets")]
    public Transform gunObject;
    public Transform[] itemObjects;

    private Vector3 gunStartPos;
    private Quaternion gunStartRot;

    private Vector3[] itemStartPositions;
    private Quaternion[] itemStartRotations;

    private bool menuActive = false;

    void Start()
    {
        // Zet menu en pointer uit bij start
        menuCanvas.SetActive(false);
        if (pointer != null) pointer.enabled = false;

        // Knoppen koppelen
        if (resetScoreButton != null)
            resetScoreButton.onClick.AddListener(ResetScore);

        if (resetAllButton != null)
            resetAllButton.onClick.AddListener(ResetAll);

        if (resetItemsButton != null)
            resetItemsButton.onClick.AddListener(ResetItems);

        // Zoek camera indien nodig
        if (playerCamera == null)
        {
            GameObject foundCam = GameObject.Find("CenterEyeAnchor");
            if (foundCam != null)
                playerCamera = foundCam.transform;
            else
                Debug.LogWarning("MenuScript: Geen CenterEyeAnchor gevonden! Zet handmatig de playerCamera in de inspector.");
        }

        // Sla originele posities van gun op
        if (gunObject != null)
        {
            gunStartPos = gunObject.position;
            gunStartRot = gunObject.rotation;
        }

        // Sla originele posities van items op
        if (itemObjects != null && itemObjects.Length > 0)
        {
            itemStartPositions = new Vector3[itemObjects.Length];
            itemStartRotations = new Quaternion[itemObjects.Length];

            for (int i = 0; i < itemObjects.Length; i++)
            {
                itemStartPositions[i] = itemObjects[i].position;
                itemStartRotations[i] = itemObjects[i].rotation;
            }
        }
    }

    void Update()
    {
        // B-knop (rechter controller)
        if (OVRInput.GetDown(OVRInput.Button.Two, OVRInput.Controller.RTouch))
        {
            ToggleMenu();
        }

        // Volg speler als menu actief is
        if (menuActive && playerCamera != null)
        {
            Vector3 forward = playerCamera.forward;
            forward.Normalize();

            Vector3 targetPosition = playerCamera.position + forward * 4.0f;
            Quaternion targetRotation = Quaternion.LookRotation(forward);

            menuCanvas.transform.position = Vector3.Lerp(menuCanvas.transform.position, targetPosition, Time.deltaTime * 5f);
            menuCanvas.transform.rotation = Quaternion.Lerp(menuCanvas.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    void ToggleMenu()
    {
        menuActive = !menuActive;
        menuCanvas.SetActive(menuActive);
        if (pointer != null) pointer.enabled = menuActive;

        if (menuActive && playerCamera != null)
        {
            Vector3 forward = playerCamera.forward;
            forward.Normalize();

            menuCanvas.transform.position = playerCamera.position + forward * 2.0f;
            menuCanvas.transform.rotation = Quaternion.LookRotation(forward);
        }
    }

    void ResetScore()
    {
        Debug.Log("Score gereset!");
        ScoreManager.Instance.ResetScore(); // Zorg dat je een ScoreManager singleton hebt
    }

    void ResetItems()
    {
        Debug.Log("Items gereset!");

        if (itemObjects != null)
        {
            for (int i = 0; i < itemObjects.Length; i++)
            {
                if (itemObjects[i] != null)
                {
                    itemObjects[i].position = itemStartPositions[i];
                    itemObjects[i].rotation = itemStartRotations[i];

                    Rigidbody rb = itemObjects[i].GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.velocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                    }
                }
            }
        }
    }

    void ResetGun()
    {
        Debug.Log("Gun gereset!");

        if (gunObject != null)
        {
            gunObject.position = gunStartPos;
            gunObject.rotation = gunStartRot;

            Rigidbody rb = gunObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    void ResetAll()
    {
        Debug.Log("Alles gereset!");
        ResetItems();
        ResetScore();
        ResetGun();
    }
}
