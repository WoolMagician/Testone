using BezierSolution;
using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class DroneSpeedPU : IPowerUP
{
    public float speedMultiplier = 2f;

    public void ApplyPowerUP(IData data)
    {
        DroneLevelData droneLevelData = (DroneLevelData)data;
        droneLevelData.maxSpeed = droneLevelData.maxSpeed * speedMultiplier;
    }
}

public class Drone : MonoBehaviour, 
    IHasPowerUPs {

    [SerializeField]
    private DroneData droneActualData;
    public DroneData droneReferenceData;
    public AmmoData ammoData;

    public Orbit orbit;
    public GameObject droneObject;
    public GameObject droneTrailObject;
    public GameObject muzzleFlashObject;

    [SerializeField]
    private List<Enemy> enemiesWithinRange = new List<Enemy>();

    [SerializeField]
    private int currentLevel = 0;

    public DroneBehaviour behaviour;
    public BezierWalkerWithSpeed walker;

    public List<IPowerUP> PowerUPs { get; set; } = new List<IPowerUP>();
    public int CurrentLevel { get => currentLevel; private set => currentLevel = value; }
    public List<Enemy> EnemiesWithinRange { get => enemiesWithinRange; }

    public DroneLevelData GetCurrentLevelData()
    {
        if (droneActualData != null)
        {
            return droneActualData.GetLevelData(currentLevel);
        }
        else
            return null;
    }

    void Start()
    {
        //Initialize drone object
        this.Initialize();
    }

    private void Update()
    {

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
            Destroy(droneObject);
            Destroy(droneTrailObject);

            if(GetCurrentLevelData() != null)
            {
                droneObject = Instantiate(GetCurrentLevelData().droneObject, this.transform);
                droneObject.transform.localScale = GetCurrentLevelData().droneObjectScaleOverride;

                if(GetCurrentLevelData().droneTrail != null)
                {
                    droneTrailObject = Instantiate(GetCurrentLevelData().droneTrail, droneObject.transform);
                }

                if (GetCurrentLevelData().droneMuzzleFlash != null)
                {
                    muzzleFlashObject = Instantiate(GetCurrentLevelData().droneMuzzleFlash, droneObject.transform);
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

    public void GetEnemiesWithinRange()
    {
        enemiesWithinRange.Clear();
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, this.GetCurrentLevelData().patrolSphereRadius, LayerMask.GetMask(EnemyFactory.Instance.ObjectName));

        foreach (Collider collider in colliders)
        {
            enemiesWithinRange.Add(collider.GetComponent<Enemy>());
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, this.GetCurrentLevelData().patrolSphereRadius);
    }

    public void ApplyDroneData()
    {
        if (droneReferenceData != null)
            droneActualData = (DroneData)droneReferenceData.Copy();                    
    }

    public void ApplyPowerUPs()
    {
        foreach (IPowerUP item in PowerUPs)
        {
            // Apply powerup chain only to current level
            item.ApplyPowerUP(GetCurrentLevelData());
        }
    }

    public void ApplyBehaviour()
    {
        if (this.behaviour != null)
            this.behaviour.Behave(this);
    }

    public void SetOrbit(Orbit orbit)
    {
        this.orbit = orbit;
        walker.spline = orbit.OrbitSpline;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer(EnemyFactory.Instance.ObjectName))
    //    {
    //        Enemy enemy = other.GetComponent<Enemy>();

    //        if (!enemiesWithinRange.Contains(enemy))
    //        {                
    //            enemiesWithinRange.Add(enemy);
    //        }
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer(EnemyFactory.Instance.ObjectName))
    //    {
    //        Enemy enemy = other.GetComponent<Enemy>();

    //        if (enemiesWithinRange.Contains(enemy))
    //        {
    //            enemiesWithinRange.Remove(enemy);
    //        }
    //    }
    //}
}
