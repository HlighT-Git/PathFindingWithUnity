using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform cameraTransform;
    private Vector3 newPosition;
    [SerializeReference] private Quaternion newRotation;
    [SerializeReference] private Vector3 newZoom;

    [SerializeReference] private Vector3 dragStartPosition;
    [SerializeReference] private Vector3 dragCurrentPosition;
    [SerializeReference] private Vector3 rotateStartPosition;
    [SerializeReference] private Vector3 rotateCurrentPosition;
    public Vector3 diff;
    void Start()
    {
        cameraTransform = GetComponentInChildren<Transform>();
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseInput();
    }
    void HandleMouseInput()
    {
        if (OnMouse.hoverABlock && Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * transform.forward;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float entry;
            if (plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);
                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        if (Input.GetMouseButton(2))
        {
            rotateCurrentPosition = Input.mousePosition;
            diff = rotateCurrentPosition - rotateStartPosition;
            rotateStartPosition = rotateCurrentPosition;
            if (Mathf.Abs(diff.x) > Mathf.Abs(diff.y))
                newRotation *= Quaternion.Euler(Vector3.up * (diff.x / 5f));
            else
                newRotation *= Quaternion.Euler(Vector3.left * (diff.y / 5f));
        }
        //transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * 2);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 2);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * 2);
    }
}
