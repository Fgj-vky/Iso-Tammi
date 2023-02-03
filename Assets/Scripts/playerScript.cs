using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    public GameObject? cameraTarget;
    public GameObject playerCamera;

    public float cameraDistance = 5f;
    public float cameraSpeed = 0.1f;

    private Vector3 focusPoint;
    private Vector2 orbitAngles = new Vector2(0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {



        if (cameraTarget != null)
        {

            UpdateFocusPoint();
            Quaternion lookRotation = Quaternion.Euler(orbitAngles);
            Vector3 lookDirection = transform.forward;
            Vector3 lookPosition = focusPoint - lookDirection * cameraDistance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }
    }


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
