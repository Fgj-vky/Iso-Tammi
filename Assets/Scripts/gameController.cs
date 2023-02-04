using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    private List<GameObject> trees = new List<GameObject>();

    [SerializeField]
    private List<EnemySpawnOption> enemies = new List<EnemySpawnOption>();

    private List<GameObject> gameEnemies = new List<GameObject>();
    private List<enemyScript> enemyScripts = new List<enemyScript>();

    [SerializeField]
    private float enemySpeed = 1f;

    [SerializeField]
    private int enemyWaveInterval = 5;
    private float enemyWaveTimer;

    private float enemyWaveSize;

    [SerializeField]
    private int enmyWaveStartSize = 5;

    [SerializeField]
    private float enenmyWaveScale = 0.25f;

    private float maxWeight = 0f;
    private float waveDegrees = 0f;

    public Transform wavePosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyWaveSize = enmyWaveStartSize;
        enemyWaveTimer = 0;

        foreach (var enemy in enemies)
        {
            maxWeight += enemy.Weight;
        }
    }

    // Update is called once per frame
    void Update()
    {
        MoveEnemies();

        enemyWaveTimer -= Time.deltaTime;
        if(enemyWaveTimer <= 0)
        {
            CreateWave(GenerateWavePosition());
            enemyWaveSize += enenmyWaveScale;
            enemyWaveTimer = enemyWaveInterval;
        }
    }

    public GameObject GetClosestTree(Vector3 pos)
    {
        if(trees.Count == 0)
        {
            return null;
        }

        GameObject closest = trees[0];
        float closestDistance = Vector3.Distance(trees[0].transform.position, pos);

        for (int i = 1; i < trees.Count; i++)
        {
            if (Vector3.Distance(trees[i].transform.position, pos) < closestDistance)
            {
                closest = trees[i];
                closestDistance = Vector3.Distance(trees[i].transform.position, pos);
            }
        }

        return closest;
    }

    public GameObject GetClosestEnemy(Vector3 pos)
    {
        if(gameEnemies.Count == 0)
        {
            return null;
        }

        GameObject closest = gameEnemies[0];
        float closestDistance = Vector3.Distance(gameEnemies[0].transform.position, pos);

        for (int i = 1; i < gameEnemies.Count; i++)
        {
            if (Vector3.Distance(gameEnemies[i].transform.position, pos) < closestDistance)
            {
                closest = gameEnemies[i];
                closestDistance = Vector3.Distance(gameEnemies[i].transform.position, pos);
            }
        }

        return closest;
    }

    private void CreateWave(Vector3 position)
    {
        waveDegrees = Mathf.PI*2 / enemyWaveSize;
        for (int i = 0; i <  (int)enemyWaveSize; i++)
        {
            float rnd = Random.Range(0f, maxWeight);

            float runningWeight = 0f;

            for (int j = 0; j < enemies.Count; j++)
            {
                if(rnd < enemies[j].Weight + runningWeight)
                {
                    float posRnd = Random.Range(-10, 10);
                    AddEnemy(Instantiate(enemies[j].Prefab, position + new Vector3(Mathf.Sin(waveDegrees * i) * posRnd, 0, Mathf.Cos(waveDegrees * i) * posRnd), Quaternion.identity));
                    break;
                }
                runningWeight += enemies[j].Weight;
            }
        }
    }

    private void AddEnemy(GameObject enemy)
    {
        gameEnemies.Add(enemy);
        enemyScripts.Add(enemy.GetComponent<enemyScript>());
    }
    private void MoveEnemies()
    {
        foreach (var enemy in enemyScripts)
        {
            enemy.Move(this);
        }
    }

    public void AddTree(GameObject tree)
    {
        trees.Add(tree);
    }

    public void RemoveTree(GameObject tree)
    {
        trees.Remove(tree);
    }

    public void RemoveEnemy(GameObject enemy, enemyScript script)
    {
        gameEnemies.Remove(enemy);
        enemyScripts.Remove(script);
    }

    private Vector3 GenerateWavePosition()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        float r = 150f;

        Vector3 pointA = new Vector3(Mathf.Sin(angle) * r, 0f, Mathf.Cos(angle) * r);
        GameObject closestTree = GetClosestTree(pointA);
        if (closestTree == null)
        {
            return pointA;
        }
        Vector3 pointB = closestTree.transform.position;

        Vector3 pointC = Vector3.Lerp(pointA, pointB, 0.5f);

        return pointC;
    }

    [System.Serializable]
    private struct EnemySpawnOption
    {
        public GameObject Prefab;
        [Range(0f, 1f)]
        public float Weight;
    }
}

