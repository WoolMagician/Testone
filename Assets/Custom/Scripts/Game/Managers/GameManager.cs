using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager> , INotificationObserver
{
    public SimulationData simulationData;

    public GameObject commonItemPrefab;
    public GameObject uncommonItemPrefab;
    public GameObject rareItemPrefab;

    public LayerMask missileRaycastMask;
    public LayerMask itemRaycastMask;
    public LayerMask planetRaycastMask;

    public GameObject gameOverScreen;

    // Start is called before the first frame update
    void Start()
    {
        Planet.Instance.OnPlanetHit += Restart;

        //Start the game with one turret
        Drone newTurret = DroneFactory.Instance.Create(0);
        newTurret.SetOrbit(OrbitFactory.Instance.CreateNewOrbit(3).GetComponent<Orbit>());
    }

    void Update()
    {
        // Need to be changed to unity touch input
        if (Input.GetMouseButtonDown(0))
        {
            //Doesn't work with post-process lens
            Ray ray = Director.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

            // If we hit a valid layermask item handle the loot
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, itemRaycastMask))
            {
                this.HandleLoot(hit.collider.gameObject.GetComponent<Loot>());
            }
            // If we hit a valid layermask target, spawn a new missile
            else if (Physics.Raycast(ray, out hit, float.MaxValue, missileRaycastMask))
            {
                this.HandleMissileSpawn(hit.transform); //Pass hit transform as target
            }
            else if (Physics.Raycast(ray, out hit, float.MaxValue, planetRaycastMask))
            {
                UIManager.Instance.ToggleDroneMenu();
            }
        }
    }

    public void OnNotify(NotificationEventArgs args)
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
        Enemy enemy = args.enemy;
        EnemyData enemyData = enemy.enemyData;
        GameObject publisher = args.publisher;

        //Increase enemy defeat count
        simulationData.defeatedEnemies += 1;

        //Instantiate loot
        LootFactory.Instance.CreateAt(enemyData.lootTable.Data, publisher.transform.position);

        //Instantiate die particles
        if (enemyData.enemyDieParticles != null)
        {
            Instantiate(enemyData.enemyDieParticles,
                        publisher.transform.position,
                        Quaternion.identity).transform.localScale = enemyData.enemyDieParticlesScaleOverride;
        }

        //Destroy game object
        Destroy(publisher);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    private void HandleLoot(Loot loot)
    {
        this.ProcessNewLoot(loot);
        Destroy(loot.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    private void HandleMissileSpawn(Transform target)
    {
        if(Planet.Instance.missilesLeft > 0)
        {
            Planet.Instance.missilesLeft -= 1;
            MissileFactory.Instance.Create(0).target = target;
        }        
    }


    #region TURRET UI REQUESTS
    
    //private void HandleTurretUINewTurret(TurretMenuItem turretMenuItem)
    //{
    //    if(acquiredMinerals >= turretMenuItem.turretData.levels[0].cost)
    //    {
    //        acquiredMinerals -= turretMenuItem.turretData.levels[0].cost;
    //        TurretFactory.CreateTurret(planet.CreateNewOrbit(orbitCount),
    //                       turretMenuItem.turretData,
    //                       turretMenuItem.ammoData);
    //    }
    //}

    #endregion

    #region LOOT

    private void HandleNewCurrencyLoot(Loot loot)
    {
        simulationData.mineralAcquired += loot.lootData.quantity;
    }

    private void HandleNewShieldLoot(Loot loot)
    {

    }

    private void HandleNewTurretLoot(Loot loot)
    {
        //TurretLootSO lootedTurret = (TurretLootSO)loot.lootData.item;

        //turretMenuUI.SetItemData(0, lootedTurret.turretData, lootedTurret.ammoData);
    }

    private void HandleNewMissileLoot(Loot loot)
    {
        if (MissileFactory.Instance.availableObjects[0].Equals((MissileSO)loot.lootData.lootSO))
        {
            Planet.Instance.missilesLeft += loot.lootData.quantity;
        }
        else
        {
            MissileFactory.Instance.availableObjects[0] = (MissileSO)loot.lootData.lootSO;
            Planet.Instance.missilesLeft = loot.lootData.quantity;
        }
    }

    /// <summary>
    /// Used to process loot data
    /// </summary>
    /// <param name="loot"></param>
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
                case LootType.Planet:
                    //Utilizzato per off-simulation
                    break;
                case LootType.Drone:
                    break;
                case LootType.Shield:
                    break;
                case LootType.Missile:
                    this.HandleNewMissileLoot(loot);
                    break;
                case LootType.Ammo:
                    //Probabilmente sempre laser ma con muzzleshot, danno e onhit effect differente??
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
        gameOverScreen.SetActive(true);
        yield return new WaitForSeconds(5);
        SceneController.Instance.LoadScene(0);
    }


}