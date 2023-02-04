using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyDeathSoundPlayerScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
