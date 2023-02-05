using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acornScript : projectileScript
{
    [SerializeField]
    private int splashDamage;
    [SerializeField]
    private float splashRadius;

    [SerializeField]
    private GameObject splashEmitter;

    protected override void onHit(GameObject enemy)
    {
        var enemiesInArea = gameController.GetEnemiesInArea(transform.position, splashRadius);

        enemiesInArea.ForEach(e => e.GetComponent<enemyScript>().GetHit(splashDamage));

        Instantiate(splashEmitter, transform.position, Quaternion.identity);

        base.onHit(enemy);
    }
}
