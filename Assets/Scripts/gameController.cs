using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameController : MonoBehaviour
{
    public List<Transform> trees = new List<Transform>();

    [SerializeField]
    private List<EnemySpawnOption> enemies = new List<EnemySpawnOption>();

    private List<Transform> gameEnemies = new List<Transform>();

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
        enemyWaveTimer = enemyWaveInterval;

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
            CreateWave(wavePosition.position);
            enemyWaveSize += enenmyWaveScale;
            enemyWaveTimer = enemyWaveInterval;
        }
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
        gameEnemies.Add(enemy.transform);
    }

    private void MoveEnemies()
    {
        foreach (var enemy in gameEnemies)
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


    [System.Serializable]
    private struct EnemySpawnOption
    {
        public GameObject Prefab;
        [Range(0f, 1f)]
        public float Weight;
    }
}

