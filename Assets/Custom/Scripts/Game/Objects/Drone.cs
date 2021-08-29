using BezierSolution;
using System.Collections.Generic;
using UnityEngine;

#region POWERUPs

[System.Serializable]
public class DroneLockOnEnemyPU : DroneSpeedPU
{
    public DroneLockOnEnemyPU(float speedMultiplier, Drone referenceDrone) : base(speedMultiplier, referenceDrone)
    {
    }

    public override void ApplyPowerUP(IData data)
    {
        if(referenceDrone.EnemiesWithinRange.Count > 0)
        {            
            base.ApplyPowerUP(data);
        }
    }
}

[System.Serializable]
public class DroneSpeedPU : BasePowerUP
{
    [SerializeField]
    protected float speedMultiplier = 1f;
    protected Drone referenceDrone;

    public DroneSpeedPU(float speedMultiplier, Drone referenceDrone)
    {
        this.speedMultiplier = speedMultiplier;
        this.referenceDrone = referenceDrone;
    }

    public override void ApplyPowerUP(IData data)
    {
        DroneLevelData droneLevelData = (DroneLevelData)data;
        droneLevelData.maxSpeed = droneLevelData.maxSpeed * speedMultiplier;
    }
}

#endregion

public class Drone : MonoBehaviour, IHasPowerUPs
{
    #region ATTRIBUTES

    [ShowOnly]
    [SerializeField]
    private bool _initialized = false;

    [SerializeField]
    public DroneData droneActualData;

    [HideInInspector]
    public DroneData droneReferenceData;

    [HideInInspector]
    public AmmoData ammoData;

    [SerializeField]
    private List<Enemy> enemiesWithinRange = new List<Enemy>();

    [HideInInspector]
    private GameObject droneObject;

    [HideInInspector]
    private GameObject droneTrailObject;

    [HideInInspector]
    public GameObject muzzleFlashObject;

    [HideInInspector]
    public Orbit orbit;

    [HideInInspector]
    public DroneBehaviour behaviour;

    [HideInInspector]
    public BezierWalkerWithSpeed walker;

    #endregion

    #region PROPERTIES

    public bool Initialized { get => _initialized; }

    public List<IPowerUP> PowerUPs { get; set; } = new List<IPowerUP>();

    public List<Enemy> EnemiesWithinRange { get => enemiesWithinRange; }

    #endregion

    private void Start()
    {
        //Initialize drone object
        this._initialized = this.Initialize();
    }

    private void Update()
    {
        //Da eliminare, solo per test
        if(Input.GetKeyDown(KeyCode.U))
        {
            this.PowerUPs.Add(new DroneLockOnEnemyPU(0.5f, this));
        }

        //Get all enemies withing patrol bounds
        this.GetEnemiesWithinRange();

        //Apply drone level data
        this.ApplyDroneData();

        //Apply powerup overrides
        this.ApplyPowerUPs();

        //Call behaviour
        this.ApplyBehaviour();
    }

    private bool Initialize()
    {
        //Apply drone level data
        this.ApplyDroneData();

        try
        {
            //Probabilmente dovrebbe essere spostato nel metodo di factory.

            Destroy(droneObject);
            Destroy(droneTrailObject);

            if(droneActualData != null)
            {
                droneObject = Instantiate(droneActualData.GetCurrentLevelData().droneObject, this.transform);
                droneObject.transform.localScale = droneActualData.GetCurrentLevelData().droneObjectScaleOverride;

                if(droneActualData.GetCurrentLevelData().droneTrail != null)
                {
                    droneTrailObject = Instantiate(droneActualData.GetCurrentLevelData().droneTrail, droneObject.transform);
                }

                if (droneActualData.GetCurrentLevelData().droneMuzzleFlashPrefab != null)
                {
                    muzzleFlashObject = Instantiate(droneActualData.GetCurrentLevelData().droneMuzzleFlashPrefab, droneObject.transform);
                }
                behaviour = droneActualData.droneBehaviourSO.GetBehaviourScript();
            }
        }
        catch
        {
            Debug.LogWarning("Drone initialization failed.");
            return false;
        }
        return true;
    }

    private void GetEnemiesWithinRange()
    {
        /* Utilizzo OverlapSphere al posto del collider
         * per ottimizzazione e migliore gestione
         * della lista dei nemici
         */

        enemiesWithinRange.Clear();
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this.droneActualData.GetCurrentLevelData().enemyDetectionRadius, LayerMask.GetMask(EnemyFactory.Instance.ObjectName));

        //Probabilmente è meglio farlo con una linQ
        foreach (Collider collider in colliders)
        {
            enemiesWithinRange.Add(collider.GetComponent<Enemy>());
        }
    }

    private void OnDrawGizmos()
    {
        //Draw enemy detection sphere
        Gizmos.DrawWireSphere(transform.position, this.droneActualData.GetCurrentLevelData().enemyDetectionRadius);
    }

    private void ApplyDroneData()
    {
        if (droneReferenceData != null)
            droneActualData = (DroneData)droneReferenceData.Copy();                    
    }

    public void ApplyPowerUPs()
    {
        if (droneActualData != null)
        {
            foreach (IPowerUP item in PowerUPs)
            {
                // Apply powerup chain only to current level
                item.ApplyPowerUP(droneActualData.GetCurrentLevelData());
            }
        }
    }

    private void ApplyBehaviour()
    {
        if (this.behaviour != null)
            this.behaviour.Behave(this);
    }

    public void SetOrbit(Orbit orbit)
    {
        this.orbit = orbit;
        walker.spline = orbit.OrbitSpline;
    }
}
