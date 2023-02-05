using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rowanProjectileScript : projectileScript
{
    [SerializeField]
    private GameObject subProjectile;
    [SerializeField]
    private float targettingRadius;

    protected override void onHit(GameObject enemy)
    {
        var enemiesInArea = gameController.GetEnemiesInArea(transform.position, targettingRadius);

        foreach (var target in enemiesInArea)
        {
            var subProj = Instantiate(subProjectile, transform.position, Quaternion.identity);
            var script = subProj.GetComponent<rowanSubProjectileScript>();
            script.SetTarget(target);
            script.gameController = gameController;
        }

        base.onHit(enemy);
    }
}
