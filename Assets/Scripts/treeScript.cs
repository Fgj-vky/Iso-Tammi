using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class treeScript : MonoBehaviour
{


    public GameObject healthBar;


    private int maxHealth = 100;
    private int health;

    private GameObject playerCamera;

    // Start is called before the first frame update
    void Start()
    {
        playerCamera = GameObject.Find("Player").transform.GetChild(0).gameObject;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            modifyHealth(-1);
        }

        healthBar.transform.parent.transform.LookAt(playerCamera.transform);

    }

    public void modifyHealth(int amount)
    {
        health += amount;
        if (health < 1)
        {
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

        healthBar.GetComponent<RectTransform>().offsetMin = new Vector2(Mathf.Lerp(maxValue, minValue, scalar), 2.31f);

    }
}
