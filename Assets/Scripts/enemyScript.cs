using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);
        transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, transform.eulerAngles.z);  
    }
    
    public void GetHit()
    {

    }
}
