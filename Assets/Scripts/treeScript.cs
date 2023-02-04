using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class treeScript : MonoBehaviour
{


    public GameObject healthBar;

    public GameObject projectile;

    public Transform projectileSpawnPoint;

    public gameController controller;

    [SerializeField]
    private float attackRate;
    private float attackTimer = 0;
    [SerializeField]
    private float attackRange;

    private int maxHealth = 100;
    private int health;

    private GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Player").transform.GetChild(0).gameObject;
        health = maxHealth;
        updateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer -= Time.deltaTime;
        // Billboard the health bars towards the camera
        healthBar.transform.parent.transform.LookAt(playerCamera.transform);
    }

    public bool CanAttack(float dist)
    {
        if (dist > attackRange)
        {
            return false;
        }
        else if (attackTimer < 0)
        {
            attackTimer = attackRate;
            return true;
        }
        return false;
    }

    public void modifyHealth(int amount)
    {
        health += amount;
        if (health < 1)
        {
            controller?.RemoveTree(gameObject);
            Destroy(gameObject);
        } else if (health > maxHealth)
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
        } else
        {
            healthBar.transform.parent.gameObject.active = true;
        }


        healthBar.GetComponent<RectTransform>().offsetMin = new Vector2(Mathf.Lerp(maxValue, minValue, scalar), 2.31f);

    }
}
