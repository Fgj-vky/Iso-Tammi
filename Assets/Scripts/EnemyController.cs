using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{

    public List<Transform> trees = new List<Transform>();

    [SerializeField]
    private List<Transform> enemies = new List<Transform>();

    [SerializeField]
    private float enemySpeed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemies();
    }

    private void MoveEnemies()
    {
        foreach (var enemy in enemies)
        {
            Transform closest = trees[0];
            float closestDistance = Vector3.Distance(trees[0].position, enemy.position);

            for (int i = 1; i < trees.Count; i++)
            {
                if(Vector3.Distance(trees[i].position, enemy.position) < closestDistance)
                {
                    closest = trees[i];
                    closestDistance = Vector3.Distance(trees[i].position, enemy.position);
                }
            }

            if (closestDistance > 0.1f)
            {
                enemy.position = Vector3.Lerp(enemy.position, closest.position, enemySpeed * 0.0001f);
            }
        }
    }
}
