using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WaveType
{
    Normal,
    Wave,
    TimedWave
}

public enum SpawnWaitTime
{
    Small,
    Medium,
    Large
}

[System.Serializable]
public class EnemyWaveData
{
    public int enemyID;
    public int quantity;
    public SpawnWaitTime nextEnemySpawnTime;
}

[System.Serializable]
public class WaveData
{
    public WaveType waveType;
    public LootTableSO lootTableSO;
    public EnemyWaveData[] enemies;
    public int killedEnemies;
    public int spawnedEnemies;

    [SerializeField]
    public bool Completed
    {
        get
        {
            return RemainingEnemies == 0;
        }
    }

    public int RemainingEnemies
    {
        get
        {
            return TotalEnemies - killedEnemies;
        }
    }

    public int TotalEnemies
    {
        get
        {
            int total = 0;

            if (enemies != null)
            {
                foreach (EnemyWaveData enemy in enemies)
                {
                    total += enemy.quantity;
                }
            }
            return total;
        }
    }

    public void Reset()
    {
        killedEnemies = 0;
    }
}

public class WaveManager : Singleton<WaveManager>, INotificationObserver
{
    public Color gizmoColor = Color.red;
    public float spawnDistanceFromSpawnPoint = 20f;
    public Transform spawnPoint;

    public int currentWaveIndex = -1;
    public WaveData currentWave;
    public List<WaveData> waves;
    public Queue<EnemyWaveData> currentWaveEnemyQueue = new Queue<EnemyWaveData>();

    private bool enemySpawnCooldown = false;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawCube(transform.position, new Vector3(0.5f, 0.5f, 0.5f));
    }

    // Start is called before the first frame update
    void Start()
    {        
        NextWave(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentWave != null && !currentWave.Completed)
        {
            switch (currentWave.waveType)
            {
                case WaveType.Normal:
                    if (currentWaveEnemyQueue != null && currentWaveEnemyQueue.Count > 0)
                    {
                        if (!enemySpawnCooldown)
                        {
                            StartCoroutine(SpawnEnemy(currentWaveEnemyQueue.Dequeue()));
                        }
                    }
                    break;
            }
        }
    }

    public static void Restart()
    {
        EnemyFactory.DestroyAllEnemies();
        Instance.currentWaveEnemyQueue.Clear();
        Instance.currentWave = null;
        Instance.currentWaveIndex = -1;
        Instance.enemySpawnCooldown = false;
        NextWave(0);
    }

    private object spawnEnemyLock = new object();

    private IEnumerator SpawnEnemy(EnemyWaveData enemyData)
    {
        //lock (spawnEnemyLock)
        //{
            float cooldownTime = 5f;
            enemySpawnCooldown = true;
            Enemy enemy = EnemyFactory.Instance.CreateAtWithRotation(enemyData.enemyID, this.GetRandomSpawnPositionWithinBounds(spawnDistanceFromSpawnPoint, 5f, -5f), Vector3.zero);

            float waveDemultiplier = 1f / (currentWaveIndex + 1f);

            switch (enemyData.nextEnemySpawnTime)
            {
                case SpawnWaitTime.Small:
                    cooldownTime = Random.Range(0.2f, 0.6f);
                    break;
                case SpawnWaitTime.Medium:
                    cooldownTime = Random.Range(4f, 6f);
                    break;
                case SpawnWaitTime.Large:
                    cooldownTime = Random.Range(10f, 14f);
                    break;
            }
            yield return new WaitForSeconds(cooldownTime * waveDemultiplier);
            enemySpawnCooldown = false;
        //}
    }

    public void NextRandomWave()
    {
        NextWave(Random.Range(0, waves.Count));
    }

    public static void NextWave(int waveIndex)
    {
        if (waveIndex < Instance.waves.Count)
        {
            Instance.currentWave = Instance.waves[waveIndex];

            if (Instance.currentWave.Completed)
            {
                Instance.currentWave.Reset();
            }

            //Prewarm
            foreach (EnemyWaveData enemy in Instance.currentWave.enemies)
            {
                for (int i = 0; i < enemy.quantity + Mathf.Round(Instance.currentWaveIndex / 3); i++)
                {
                    Instance.currentWaveEnemyQueue.Enqueue(enemy);
                }
            }
            Debug.Log(string.Format("Wave '{0}' prewarm.", Instance.currentWaveIndex));
        }
        else
        {
            Instance.currentWave = null;
            Debug.Log("No more waves available.");
        }
    }

    private Vector3 GetRandomSpawnPositionWithinBounds(float radius, float upperLimit, float lowerLimit)
    {
        Vector2 randomInsideCircle = Random.insideUnitCircle.normalized;
        Vector3 spawnPos = spawnPoint.position + new Vector3(randomInsideCircle.x,
                                                             0,
                                                             randomInsideCircle.y) * radius;
        spawnPos.y += Random.Range(lowerLimit, upperLimit);
        return spawnPos;
    }

    public void OnNotification(NotificationEventArgs args)
    {
        System.Type type = args.GetType();

        switch (type)
        {
            case System.Type t when type == typeof(EnemyDeathNotificationEventArgs):
                this.HandleEnemyDeath((EnemyDeathNotificationEventArgs)args);
                break;
            default:
                break;
        }
    }

    private void HandleEnemyDeath(EnemyDeathNotificationEventArgs args)
    {
        // Increase killed enemies for the current wave
        currentWave.killedEnemies += 1;

        // If current wave is completed, roll wave loot table
        if (currentWave.Completed)
        {
            if (this.currentWave.lootTableSO != null && this.currentWave.lootTableSO.Data != null)
            {
                // Instantiate loot for the current completed wave
                LootFactory.Instance.CreateAtWithRotation(this.currentWave.lootTableSO.Data, args.enemy.transform.position, new Vector3(-90, 0, 0));
            }

            // Inc wave index
            Instance.currentWaveIndex += 1;

            // Throw next wave
            NextRandomWave();

        }
    }

    public void SubscribeTo(NotificationPublisher publisher)
    {
        publisher.AddObserver(this);
    }

    public void UnsubscribeFrom(NotificationPublisher publisher)
    {
        publisher.RemoveObserver(this);
    }
}