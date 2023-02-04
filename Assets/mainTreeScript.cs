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
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale *= (1 + scaleFactor * Time.deltaTime);
    }
}
