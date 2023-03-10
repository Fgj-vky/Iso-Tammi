using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript : MonoBehaviour
{

    private GameObject? cameraTarget;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private GameObject[] treePrefabs;

    [SerializeField]
    private float cameraHeight = 10f;
    [SerializeField]
    private float cameraDistance = 5f;
    [SerializeField]
    private float cameraSpeed = 0.1f;
    [SerializeField]
    private float mouseSpeed = 0.2f;
    [SerializeField]
    private float minCameraDistance = 3f;
    [SerializeField]
    private float maxCameraDistance = 20f;

    [SerializeField]
    private GameObject pauseOverlay;

    private treeScript targetTreeScript;
    private gameController gameController;
    private hotBarController hotBarController;

    private Vector3 focusPoint;
    private Vector2 orbitAngles = new Vector2(0f, 0f);
    [SerializeField]
    private float maxCameraAngle = 10f;
    [SerializeField]
    private float minCameraAngle = -20f;
    private bool mouseDragging = false;
    private Vector2 dragDir = Vector2.zero;

    private int cardIndex = -1; // -1 is no card fyi

    [SerializeField]
    private int maxCardCount = 4;
    [SerializeField]
    private int cardThreshold;
    private int cardPoints;
    [SerializeField]
    private float cardTimeout = 0.5f;
    private float cardTimer;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        hidePauseOverlay();
        cardTimer = cardTimeout;
        cardPoints = 0;
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        hotBarController = GameObject.FindGameObjectWithTag("HotBarController").GetComponent<hotBarController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Handle mouse drag
        cardTimer -= Time.deltaTime;
        if(cardTimer < 0)
        {
            cardTimer = cardTimeout;
            if(cardPoints >= cardThreshold && hotBarController.CardCount < maxCardCount)
            {
                hotBarController.AddRandomCard();
                cardPoints -= cardThreshold;
            }
        }

        if (Input.GetMouseButton(1) && !paused)
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


        if (Input.GetMouseButtonDown(0) && !paused)
        {
            RaycastHit hit;
            Ray ray = playerCamera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();
            if (Physics.Raycast(ray, out hit, 1000f))
            {
                if (hit.collider.gameObject.tag == "Ground" && !isOverUI && cardIndex > -1)
                {
                    GameObject tree = Instantiate(treePrefabs[cardIndex], hit.point, Quaternion.identity);
                    this.gameController.AddTree(tree);
                    tree.GetComponent<treeScript>().controller = gameController;
                    cardIndex = -1;
                    hotBarController.GetComponent<hotBarController>().ClearSelectedCard();
                }
            }


        }

        // Zoom camera
        float mouseScroll = Input.mouseScrollDelta.y;
        if (mouseScroll != 0)
        {
            cameraDistance -= mouseScroll;
            if (cameraDistance > maxCameraDistance)
            {
                cameraDistance = maxCameraDistance;
            } else if(cameraDistance < minCameraDistance) {
                cameraDistance = minCameraDistance;
            }
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

        if(Input.GetKey(KeyCode.Space))
        {
            Attack();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;

            if (paused)
            {
                Time.timeScale = 0f;
                showPauseOverlay();
            }
            else
            {
                Time.timeScale = 1f;
                hidePauseOverlay();
            }

            //UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
            //UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("testing-scene");
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
            focusPoint = Vector3.Lerp(currentPoint, targetPoint, cameraSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        targetTreeScript.Attack();
    }

    public void SetCardIndex(int index)
    {
        cardIndex = index;
    }

    public void AddPoints(int amount)
    {
        cardPoints += amount;
    }

    public void showPauseOverlay()
    {
        pauseOverlay.active = true;
    }

    public void hidePauseOverlay()
    {
        pauseOverlay.active = false;
    }
}
