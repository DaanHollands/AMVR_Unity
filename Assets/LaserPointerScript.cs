using UnityEngine;
using UnityEngine.EventSystems;

public class LaserPointerScript : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public GameObject dot;
    public float maxDistance = 10f;

    private Camera uiCamera;
    private EventSystem eventSystem;
    private PointerEventData pointerEventData;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();

        eventSystem = EventSystem.current;
        pointerEventData = new PointerEventData(eventSystem);

        // Zorg dat er een camera is die UI raycast kan doen
        uiCamera = Camera.main;
        if (uiCamera == null)
            Debug.LogWarning("LaserPointer: Geen Main Camera gevonden voor UI raycast.");
    }

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Vector3 endPosition = transform.position + transform.forward * maxDistance;

        // Raycast op Physics en UI tegelijk
        bool hitUI = false;

        // UI raycast
        pointerEventData.position = uiCamera.WorldToScreenPoint(transform.position + transform.forward * maxDistance);
        var results = new System.Collections.Generic.List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, results);

        if (results.Count > 0)
        {
            // UI hit
            endPosition = uiCamera.ScreenToWorldPoint(new Vector3(results[0].screenPosition.x, results[0].screenPosition.y, maxDistance));
            hitUI = true;
        }
        else if (Physics.Raycast(ray, out hit, maxDistance))
        {
            // 3D object hit
            endPosition = hit.point;
        }

        // Update line renderer en dot
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, endPosition);

        if (hitUI || Physics.Raycast(ray, maxDistance))
        {
            dot.SetActive(true);
            dot.transform.position = endPosition;
            dot.transform.LookAt(transform); // Richt dot naar hand
        }
        else
        {
            dot.SetActive(false);
        }
    }
}
