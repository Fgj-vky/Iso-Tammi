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
    protected void Start()
    {
        spriteRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (var renderer in spriteRenderers)
        {
            renderer.sprite = projectileSprite;
        }
    }

    // Update is called once per frame
    protected void Update()
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

        var enemy = getTarget();

        if(enemy == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.LookAt(enemy.transform);

        float distance = Vector3.Distance(enemy.transform.position, transform.position);

        if (distance > 1.5f)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position + new Vector3(0, 1f, 0), Time.deltaTime * speed);
        }
        else
        {
            onHit(enemy);
        }
    }

    protected virtual GameObject getTarget()
    {
        return gameController.GetClosestEnemy(transform.position);
    }

    protected virtual void onHit(GameObject enemy)
    {
            enemy.GetComponent<enemyScript>().GetHit(damage);
            foreach (var renderer in spriteRenderers)
            {
                renderer.enabled = false;
                delete = true;
            }
    }
}
