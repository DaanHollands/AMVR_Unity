using UnityEngine;
using UnityEngine.EventSystems;

public class LaserPointerClick : MonoBehaviour
{
    public OVRInput.Button clickButton = OVRInput.Button.One; // A knop
    public Camera uiCamera;

    private EventSystem eventSystem;
    private PointerEventData pointerEventData;

    void Start()
    {
        eventSystem = EventSystem.current;
        pointerEventData = new PointerEventData(eventSystem);

        if (uiCamera == null)
            uiCamera = Camera.main;
    }

    void Update()
    {
        pointerEventData.position = uiCamera.WorldToScreenPoint(transform.position + transform.forward * 10f);

        if (OVRInput.GetDown(clickButton))
        {
            var results = new System.Collections.Generic.List<RaycastResult>();
            eventSystem.RaycastAll(pointerEventData, results);

            foreach (var result in results)
            {
                ExecuteEvents.Execute(result.gameObject, pointerEventData, ExecuteEvents.pointerClickHandler);
            }
        }
    }
}
