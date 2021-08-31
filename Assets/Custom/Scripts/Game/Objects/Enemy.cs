using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Enemy : NotificationPublisher, IHasHealth , IHasPowerUPs
{
    public EnemyData enemyActualData;
    public EnemyData enemyReferenceData;
    public EnemyBehaviour behaviour;

    public event HealthChangedArgs OnHealthChanged;

    private float _health;

    public float Health {
        get => _health;
        set {
            float oldValue = _health;
            _health = value;
            OnHealthChanged?.Invoke(oldValue, _health);
            if (this.Health == 0) this.Die();
        }
    }

    public List<IPowerUP> PowerUPs { get; set; } = new List<IPowerUP>();

    private void Update()
    {
        //Apply enemy data
        this.ApplyEnemyData();

        //Apply powerup overrides
        this.ApplyPowerUPs();

        //Call behaviour
        this.ApplyBehaviour();
    }

    public void ApplyEnemyData()
    {
        if (enemyReferenceData != null)
            enemyActualData = (EnemyData)enemyReferenceData.Copy();
    }

    public void ApplyPowerUPs()
    {
        foreach (IPowerUP item in PowerUPs)
        {
            // Apply powerup chain only to current level
            item.ApplyPowerUP(enemyActualData, this);
        }
    }

    public void ApplyBehaviour()
    {
        if (this.behaviour != null)     
            this.behaviour.Behave(this);        
    }

    public void SetEnemyData(EnemyData enemyData)
    {
        enemyReferenceData = enemyData;
        this.behaviour = enemyReferenceData.enemyBehaviourSO.Behaviour;
        this._health = enemyReferenceData.maxHealth;
    }

    public void SetHealth(float healthValue)
    {
        this.Health = healthValue;
    }

    public void IncreaseHealth(float value)
    {
        this.Health += value;
    }

    public void DecreaseHealth(float value)
    {
        this.Health = Mathf.Clamp(this.Health - value, 0, this.enemyActualData.maxHealth);
    }

    private object lockObj = new object();

    public void Die()
    {
        lock(lockObj)
        {
            //Request loot
            this.Notify(new LootRequestNotificationEventArgs(Instantiate(this.enemyActualData.lootTableSO).Data, this.gameObject));

            //Notify death
            this.Notify(new EnemyDeathNotificationEventArgs(this));

            //Instantiate die particles
            if (enemyActualData.enemyDieParticles != null)
            {
                Instantiate(enemyActualData.enemyDieParticles,
                            this.transform.position,
                            Quaternion.identity).transform.localScale = enemyActualData.enemyDieParticlesScaleOverride;
            }

            //Destroy game object
            Destroy(this.gameObject);
        }
    }
}
