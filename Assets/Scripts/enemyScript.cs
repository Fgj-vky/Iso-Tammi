using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyScript : MonoBehaviour
{
    private Transform playerTransform;

    [SerializeField]
    private float speed;

    [SerializeField]
    private int damage;
    [SerializeField]
    private float attackRate;
    private float attackTimer = 0;

    [SerializeField]
    private int maxHealth = 100;
    private int health;
    [SerializeField]
    private GameObject healthBar;

    private GameObject? target = null;
    private treeScript? targetTreeScript = null;

    private gameController controller;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Camera").transform;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(playerTransform);
        transform.eulerAngles = new Vector3 (0, transform.eulerAngles.y, transform.eulerAngles.z);
        healthBar.transform.parent.transform.LookAt(playerTransform);
    }

    public void Move(gameController controller)
    {
        this.controller = controller;
        var closest = controller.GetClosestTree(transform.position);
        
        attackTimer -= Time.deltaTime;

        if (closest == null)
        {
            return;
        }

        if(target != closest)
        {
            target = closest;
            targetTreeScript = target.GetComponent<treeScript>();
        }

        float closestDistance = Vector3.Distance(transform.position, closest.transform.position);


        if (closestDistance > 4f)
        {
            transform.position = Vector3.MoveTowards(transform.position, closest.transform.position, speed * Time.deltaTime);
        }
        else if(attackTimer < 0)
        {
            targetTreeScript?.modifyHealth(-damage);
            attackTimer = attackRate;
        }

    }
    
    public void GetHit(int damage)
    {

        health -= damage;
        if (health < 1)
        {
            controller.RemoveEnemy(gameObject, this);
            Destroy(gameObject);
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
        updateHealthBar();
    }

    private void updateHealthBar()
    {
        float scalar = health / (maxHealth * 1.0f);
        float maxValue = 6.55f;
        float minValue = 3.453f;


        if (health == maxHealth)
        {
            healthBar.transform.parent.gameObject.active = false;
        }
        else
        {
            healthBar.transform.parent.gameObject.active = true;
        }


        healthBar.GetComponent<RectTransform>().offsetMin = new Vector2(Mathf.Lerp(maxValue, minValue, scalar), 2.31f);

    }
}
