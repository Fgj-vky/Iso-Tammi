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

    private GameObject? target = null;
    private treeScript? targetTreeScript = null;

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

    public void Move(gameController controller)
    {

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
    
    public void GetHit()
    {

    }
}
