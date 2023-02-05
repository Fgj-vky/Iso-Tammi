using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainTreeScript : MonoBehaviour
{
    [SerializeField]
    private float scaleFactor;

    public bool gameWon = false;

    // Start is called before the first frame update
    void Start()
    {
        var contorller = GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>();
        GetComponent<treeScript>().controller = contorller;
        contorller.AddTree(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= (1 + scaleFactor * Time.deltaTime);
    }

    private void OnDestroy()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex != 1)
        {
            return;
        }
        if(!gameWon)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOverScene");
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("testing-scene");

        }
    }
}
