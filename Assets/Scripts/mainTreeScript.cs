using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainTreeScript : MonoBehaviour
{
    [SerializeField]
    private float scaleFactor;

    // Start is called before the first frame update
    void Start()
    {
        GameObject.FindGameObjectWithTag("GameController").GetComponent<gameController>().AddTree(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= (1 + scaleFactor * Time.deltaTime);
    }

    private void OnDestroy()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("testing-scene");
    }
}
