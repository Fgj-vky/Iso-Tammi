
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birchScript : treeScript
{
    [SerializeField]
    private int projectileAmount;

    [SerializeField]
    private float projectileRadius;

    private float projectileDegrees;

    protected new void Start()
    {
        projectileDegrees = Mathf.PI * 2 / projectileAmount;
        base.Start();
    }

    protected override bool CanAttack()
    {
        if (transform == null)
        {
            return false;
        }
        if (attackTimer < 0)
        {
            attackTimer = attackRate;
            return true;
        }
        return false;
    }

    public override void Attack()
    {
        if (!CanAttack())
        {
            return;
        }
       
        for(int i = 0; i < projectileAmount; i++)
        {
            var projectile = Instantiate(this.projectile, projectileSpawnPoint.position + new Vector3(Mathf.Sin(projectileDegrees * i), 0, Mathf.Cos(projectileDegrees * i)), Quaternion.identity);
            projectile.GetComponent<birchProjectileScript>().gameController = controller;
        }
        PlayShootSound();
    }
}
