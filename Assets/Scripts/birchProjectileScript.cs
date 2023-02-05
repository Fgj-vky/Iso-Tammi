using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birchProjectileScript : projectileScript
{
    [SerializeField]
    private float idleLifeTime;
    private float idleTimer;
    private GameObject? target = null;
    private bool startMoving;

    [SerializeField]
    private float idleSpeed;

    [SerializeField]
    private float range;


    protected new void Start()
    {
        idleTimer = idleLifeTime;
        base.Start();
    }

    // Update is called once per frame
    protected new void Update()
    {
        if(!startMoving)
        {
            idleTimer -= Time.deltaTime;
            if(idleTimer < 0)
            {
                Destroy(gameObject);
            }

            var closestTarget = gameController.GetClosestEnemy(transform.position, range);
            if(closestTarget != null)
            {
                target = closestTarget;
                startMoving = true;
                return;
            }

            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, 0, transform.position.z), Time.deltaTime * idleSpeed);

            return;
        }
        base.Update();
    }

    protected override GameObject getTarget()
    {
        if(target == null)
        {
            target = gameController.GetClosestEnemy(transform.position, range);
        }

        return target;
    }
}
