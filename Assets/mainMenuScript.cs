using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("testing-scene");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MainMenu");
    }
    public void Credits()
    {

    }
    public void Quit()
    {
        Application.Quit();
    }
}
