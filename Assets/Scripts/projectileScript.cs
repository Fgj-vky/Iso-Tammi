using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileScript : MonoBehaviour
{

    public gameController? gameController = null;

    [SerializeField]
    private Sprite projectileSprite;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int damage;

    private SpriteRenderer[] spriteRenderers;


    float deleteTimer = 5;
    bool delete = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
        {
            renderer.sprite = projectileSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameController == null) return;

        if(delete)
        {
            deleteTimer -= Time.deltaTime;
            if(deleteTimer < 0)
            {
                Destroy(gameObject);
            }
            return;
        }

        var enemy = gameController.GetClosestEnemy(transform.position);

        if(enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.LookAt(enemy.transform);

        float distance = Vector3.Distance(enemy.transform.position, transform.position);

        if (distance > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, speed * 0.01f);
        }
        else
        {
            enemy.GetComponent<enemyScript>().GetHit(damage);
            foreach (var renderer in spriteRenderers)
            {
                renderer.enabled = false;
                delete = true;
            }

        }
    }
}
