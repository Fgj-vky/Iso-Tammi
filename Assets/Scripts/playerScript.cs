using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    private GameObject? cameraTarget;
    [SerializeField]
    private GameObject playerCamera;

    [SerializeField]
    private float cameraHeight = 10f;
    [SerializeField]
    private float cameraDistance = 5f;
    [SerializeField]
    private float cameraSpeed = 0.1f;
    [SerializeField]
    private float mouseSpeed = 0.2f;

    private treeScript targetTreeScript;
    private gameController gameController;

    private Vector3 focusPoint;
    private Vector2 orbitAngles = new Vector2(0f, 0f);
    private float maxCameraAngle = 10f;
    private float minCameraAngle = -20f;
    private bool mouseDragging = false;
    private Vector2 dragDir = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
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
            orbitAngles = new Vector2(vAngle, orbitAngles.y - dragDir.x * mouseSpeed);
            dragDir = mousePos;
        }
        else
        {
            mouseDragging = false;
            dragDir = Vector2.zero;
        }


  

        // Move and rotate the camera
        UpdateFocusPoint();
        if (cameraTarget != null)
        {

            Quaternion lookRotation = Quaternion.Euler(orbitAngles);
            Vector3 lookDirection = transform.forward;
            Vector3 lookPosition = focusPoint - lookDirection * cameraDistance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }

        // Camera offset
        playerCamera.transform.localPosition = new Vector3(0f, cameraDistance + cameraHeight, -cameraDistance);
    }

    public void ChangeTarget(GameObject newTarget)
    {
        cameraTarget = newTarget;
        targetTreeScript = newTarget.GetComponent<treeScript>();
    }

    // Tween the camera movement 
    private void UpdateFocusPoint()
    {

        Vector3 currentPoint = transform.position + transform.forward * cameraDistance;

        if (cameraTarget == null)
        {
            GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
            if (trees.Length > 0)
            {

                float closestDistance = -1;
                GameObject closestTree = null;

                for (int i = 0; i < trees.Length; i++) {
                    if (closestDistance < 0)
                    {
                        closestDistance = Vector3.Distance(trees[i].transform.position, currentPoint);
                        closestTree = trees[i];
                    }

                    float d = Vector3.Distance(trees[i].transform.position, currentPoint);

                    if(d < closestDistance)
                    {
                        closestDistance = d;
                        closestTree = trees[i];
                    }
                }
                ChangeTarget(closestTree);


            } else
            {
                cameraTarget = null;
                return;
            }
        }
        Vector3 targetPoint = cameraTarget.transform.position;

        float distance = Vector3.Distance(targetPoint, currentPoint);

        if (distance > 0.1f)
        {
            focusPoint = Vector3.Lerp(currentPoint, targetPoint, cameraSpeed);
        }
    }

    private void Attack()
    {
        var projectile = Instantiate(targetTreeScript.projectile, targetTreeScript.projectileSpawnPoint.position, Quaternion.identity);
        projectile.GetComponent<projectileScript>().gameController = gameController;
    }
}
