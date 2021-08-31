using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> , INotificationObserver
{
    public SimulationData simulationData = new SimulationData();
    public SMSimulation smSimulation = new SMSimulation();

    // Start is called before the first frame update
    void Start()
    {
        Planet.Instance.OnPlanetHit += Restart;

        //Start the game with one turret
        Drone newTurret = DroneFactory.Instance.Create(0);
        newTurret.SetOrbit(OrbitFactory.Instance.Create(0));

        //Attach event handlers
        InputEventManager.Instance.OnLootClick += Instance.HandleLootClick;
        InputEventManager.Instance.OnEnemyClick += Instance.HandleEnemyClick;
        InputEventManager.Instance.OnPlanetClick += Instance.HandlePlanetClick;

        simulationData.mineralAcquired = 10;
    }

    void Update()
    {
        // Update simulation state machine
        smSimulation.Update();
    }

    public void OnNotification(NotificationEventArgs args)
    {
        Type type = args.GetType();

        switch (type)
        {
            case Type t when type == typeof(EnemyDeathNotificationEventArgs):
                this.HandleEnemyDeath((EnemyDeathNotificationEventArgs)args);
                break;
            default:
                break;
        }
    }

    private void HandleEnemyDeath(EnemyDeathNotificationEventArgs args)
    {
        //Increase enemy defeat count
        simulationData.defeatedEnemies += 1;
    }

    private void HandleLootClick(Loot loot)
    {
        this.ProcessNewLoot(loot);
        Destroy(loot.gameObject);
    }

    private void HandleEnemyClick(Enemy enemy)
    {
        if(this.simulationData.missilesLeft > 0)
        {
            this.simulationData.missilesLeft -= 1;
            Missile newMissile = MissileFactory.Instance.CreateAt(0, Planet.Instance.transform.position);
            newMissile.transform.LookAt(enemy.transform);
            newMissile.target = enemy.transform;
        }        
    }

    private void HandlePlanetClick(Planet planet)
    {
        UIManager.Instance.ToggleDroneMenu();
    }

    #region LOOT

    private void HandleNewCurrencyLoot(Loot loot)
    {
        this.simulationData.mineralAcquired += loot.lootData.quantity;
    }

    private void HandleNewShieldLoot(Loot loot)
    {

        if (this.simulationData.shieldHitsLeft + loot.lootData.quantity <= 3)
        {
            this.simulationData.shieldHitsLeft += loot.lootData.quantity;

            if (!Planet.Instance.shield.shieldActive)
                StartCoroutine(Planet.Instance.shield.ActivateAfterTime(0));
        }

    }

    private void HandleNewMissileLoot(Loot loot)
    {
        //if (MissileFactory.Instance.availableObjects[0].Equals((MissileSO)loot.lootData.lootSO))
        {
            if (this.simulationData.missilesLeft + loot.lootData.quantity <= 5)
            {
                this.simulationData.missilesLeft += loot.lootData.quantity;
            }
        }
        /* UNCOMMENT IN CASE OF MULTI-MISSILE MANAGEMENT
        else
        {            
            MissileFactory.Instance.availableObjects[0] = (MissileSO)loot.lootData.lootSO;
            this.simulationData.missilesLeft = loot.lootData.quantity;            
        }
        */
    }

    private void HandleNewPowerupLoot(Loot loot)
    {
        foreach (Drone drone in DroneFactory.Instance.CreatedObjects)
        {
            if(drone != null)
            {
                //Add on hit effect if is not applied yet
                if (!drone.PowerUPs.ContainsOfType(loot.lootData.lootSO))
                {
                    drone.PowerUPs.Add(((PowerUPSO)loot.lootData.lootSO).PowerUP);
                }
                else
                {
                    ((BasePowerUP)drone.PowerUPs.GetOfType(loot.lootData.lootSO)).StackDuration();
                }
            }
        }
    }

    private void ProcessNewLoot(Loot loot)
    {
        if(loot != null)
        {
            //Debug loot name
            Debug.Log(loot.lootData.name);

            //Get loot type
            Type type = loot.lootData.GetType();

            //Switch on loot type
            switch (loot.lootData.type)
            {
                case LootType.Resource:
                    this.HandleNewCurrencyLoot(loot);
                    break;
                case LootType.Shield:
                    this.HandleNewShieldLoot(loot);
                    break;
                case LootType.Missile:
                    this.HandleNewMissileLoot(loot);
                    break;
                case LootType.Powerup:
                    this.HandleNewPowerupLoot(loot);
                    break;
                default:
                    Debug.LogWarning(string.Format("Loot type '{0}' is not supported.", type.Name));
                    break;
            }
        }
        else
        {
            Debug.LogWarning("Cannot process a null loot.");
        }
    }

    #endregion

    private void Restart(GameObject hitObject)
    {
        StartCoroutine(Restart());
    }

    private IEnumerator Restart()
    {
        //gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneController.Instance.LoadScene(0);
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