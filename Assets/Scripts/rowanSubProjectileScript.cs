using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rowanSubProjectileScript : projectileScript
{
    private GameObject target;

    public void SetTarget(GameObject trg)
    {
        target = trg;
    }

    protected override GameObject getTarget()
    {
        return target;
    }
}
