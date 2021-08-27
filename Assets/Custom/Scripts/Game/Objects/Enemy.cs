using UnityEngine;

public class EnemyDeathNotificationEventArgs : NotificationEventArgs
{
    public Enemy enemy;
    
    public EnemyDeathNotificationEventArgs(Enemy enemy)
    {
        this.publisher = enemy.gameObject;
        this.enemy = enemy;
    }
}

public class Enemy : MonoBehaviour, IHasHealth
{
    public EnemyData enemyData;
    public EnemyBehaviour currentEnemyBehaviour;

    private float _health;

    public float Health {
        get => _health;
        set {
            float oldValue = _health;
            _health = value;
            OnHealthChanged?.Invoke(oldValue, _health);
        }
    }

    public event HealthChangedArgs OnHealthChanged;

    private NotificationPublisher publisher = new NotificationPublisher();

    public void SetEnemyData(EnemyData enemyData)
    {
        this.enemyData = enemyData;
        this.Health = this.enemyData.maxHealth;

        if(enemyData.enemyBehaviourSO != null)
        {
            this.currentEnemyBehaviour = enemyData.enemyBehaviourSO.GetBehaviourScript();
        }
    }

    public void SetEnemyBehaviour(EnemyBehaviour enemyBehaviour)
    {
        currentEnemyBehaviour = enemyBehaviour;
    }

    private void Start()
    {
        //Add observers
        publisher.AddObserver(WaveManager.Instance);

        //Keep game manager as last as it will destroy this object.
        publisher.AddObserver(GameManager.Instance);
    }

    void Update()
    {
        this.currentEnemyBehaviour.Behave(this);
    }    

    public void SetHealth(float healthValue)
    {
        throw new System.NotImplementedException();
    }

    public void DecreaseHealth(float value)
    {
        this.Health = Mathf.Clamp(this.Health - value, 0, this.enemyData.maxHealth);

        if (this.Health == 0)
        {
            publisher.Notify(new EnemyDeathNotificationEventArgs(this));
        }
    }

    public void IncreaseHealth(float value)
    {
        throw new System.NotImplementedException();
    }
}
