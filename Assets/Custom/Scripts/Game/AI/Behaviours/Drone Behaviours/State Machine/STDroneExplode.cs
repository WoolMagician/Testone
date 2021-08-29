using System.Collections;
using UnityEngine;

public class STDroneExplode : DroneState
{
    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (drone.EnemiesWithinRange.Count > 0)
        {
            drone.StartCoroutine(Explode());
        }
    }

    private IEnumerator Explode()
    {
        foreach (Enemy enemy in drone.EnemiesWithinRange)
        {
            enemy.DecreaseHealth(drone.droneActualData.GetCurrentLevelData().damageOnDeath);
        }
        Object.Destroy(drone.gameObject);
        yield return new WaitForSeconds(0);
    }
}