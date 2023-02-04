using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    public GameObject? cameraTarget;
    public GameObject playerCamera;

    public float cameraHeight = 10f;
    public float cameraDistance = 5f;
    public float cameraSpeed = 0.1f;
    public float mouseSpeed = 0.2f;

    private Vector3 focusPoint;
    private Vector2 orbitAngles = new Vector2(0f, 0f);
    private float maxCameraAngle = 10f;
    private float minCameraAngle = -20f;
    private bool mouseDragging = false;
    private Vector2 dragDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Handle mouse drag

        if (Input.GetMouseButton(1))
        {
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            if (!mouseDragging)
            {
                dragDir = mousePos;
            }
            mouseDragging = true;
            dragDir = dragDir - mousePos;

            float vAngle = Mathf.Max(Mathf.Min(orbitAngles.x + dragDir.y * mouseSpeed, maxCameraAngle), minCameraAngle);
            orbitAngles = new Vector2(vAngle, orbitAngles.y + dragDir.x * mouseSpeed);
            dragDir = mousePos;
        }
        else
        {
            mouseDragging = false;
            dragDir = Vector2.zero;
        }


        // Move and rotate the camera
        if (cameraTarget != null)
        {

            UpdateFocusPoint();
            Quaternion lookRotation = Quaternion.Euler(orbitAngles);
            Vector3 lookDirection = transform.forward;
            Vector3 lookPosition = focusPoint - lookDirection * cameraDistance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        // Camera offset
        playerCamera.transform.localPosition = new Vector3(0f, cameraDistance + cameraHeight, -cameraDistance);
    }

    // Tween the camera movement 
    private void UpdateFocusPoint()
    {
        Vector3 targetPoint = cameraTarget.transform.position;
        Vector3 currentPoint = transform.position + transform.forward * cameraDistance;

        float distance = Vector3.Distance(targetPoint, currentPoint);

        if (distance > 0.1f)
        {
            focusPoint = Vector3.Lerp(currentPoint, targetPoint, cameraSpeed);
        }
    }
}
