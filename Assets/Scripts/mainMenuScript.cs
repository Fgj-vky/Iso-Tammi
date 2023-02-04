using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuScript : MonoBehaviour
{
    [SerializeField]
    private Camera camera;

    private float cameraMenuAngle = -14.5f;
    private float cameraCreditsAngle = -90f;
    private bool showingCredits = false;
    private float t = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float targetAngle = cameraMenuAngle;
        if (showingCredits)
        {
            targetAngle = cameraCreditsAngle;
        }


        Quaternion cameraTargetRotation = Quaternion.Euler(new Vector2(targetAngle, 0f));
        Quaternion cameraCurrentRotation = camera.transform.rotation;

        camera.transform.rotation = Quaternion.Lerp(cameraCurrentRotation, cameraTargetRotation, 0.05f);

    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("testing-scene");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MainMenu");
    }
    public void Credits()
    {
        showingCredits = !showingCredits;
    }
    public void Quit()
    {
        Application.Quit();
    }


}
