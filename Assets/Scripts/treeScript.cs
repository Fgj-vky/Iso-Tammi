using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class treeScript : MonoBehaviour
{


    public GameObject healthBar;

    [SerializeField]
    protected GameObject projectile;

    public Transform projectileSpawnPoint;

    public gameController controller;

    [SerializeField]
    protected float attackRate;
    [SerializeField]
    private float idleAttackRate;
    protected float attackTimer = 0;
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private GameObject deathAudioSourcePrefab;

    private int maxHealth = 100;
    private int health = 1000;

    private GameObject playerCamera;

    // Start is called before the first frame update
    protected void Start()
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

        if (attackTimer <= attackRate - idleAttackRate)
        {
            Attack();
        }
    }

    protected virtual bool CanAttack()
    {
        if (transform == null)
        {
            return false;
        }

        var closestE = controller.GetClosestEnemy(transform.position);

        if (closestE == null)
        {
            return false;
        }

        var dist = Vector3.Distance(transform.position, closestE.transform.position);

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
            Instantiate(deathAudioSourcePrefab, transform.position, Quaternion.identity);
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

    public void PlayShootSound()
    {
        audioSource.Play();
    }

    public virtual void Attack()
    {
        if (!CanAttack())
        {
            return;
        }

        var projectile = Instantiate(this.projectile, projectileSpawnPoint.position, Quaternion.identity);
        PlayShootSound();
        projectile.GetComponent<projectileScript>().gameController = controller;
    }
}
