using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{

    [SerializeField]
    private Sprite projectileSprite;

    [SerializeField]
    private GameObject target;
    private Transform targetTransform;
    private enemyScript targetController;

    [SerializeField]
    private float speed;

    private SpriteRenderer[] spriteRenderers;


    float deleteTimer = 5;
    bool delete = false;

    // Start is called before the first frame update
    void Start()
    {
        targetTransform = target.transform;
        targetController = target.GetComponent<enemyScript>();

        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
        {
            renderer.sprite = projectileSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(delete)
        {
            deleteTimer -= Time.deltaTime;
            if(deleteTimer < 0)
            {
                Destroy(gameObject);
                return;
            }
        }

        transform.LookAt(targetTransform);

        float distance = Vector3.Distance(targetTransform.position, transform.position);

        Vector3 unitVector = (targetTransform.position - transform.position) / Vector3.Distance(transform.position, targetTransform.position);

        if (distance > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetTransform.position, speed * 0.01f);
        }
        else
        {
            targetController.GetHit();
            foreach (var renderer in spriteRenderers)
            {
                renderer.enabled = false;
                delete = true;
            }

        }
    }
}
