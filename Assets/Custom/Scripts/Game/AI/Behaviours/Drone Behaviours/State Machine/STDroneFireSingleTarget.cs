using DG.Tweening;
using UnityEngine;
using System.Collections;

public class STDroneFireSingleTarget : STDroneMove
{
    Enemy currentFireTarget;
    bool firingCooldown = false;

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        base.UpdateState();

        if (drone.EnemiesWithinRange.Count > 0)
        {
            if (currentFireTarget == null)
            {
                //Take first enemy in range as target
                currentFireTarget = drone.EnemiesWithinRange[0];
            }
        }
        else
        {
            currentFireTarget = null;
            drone.LockedEnemiesForCombat.Clear();
        }

        //Fire logic
        if (currentFireTarget != null)
        {
            //Smoothly turn toward enemy
            drone.transform.DOLookAt(currentFireTarget.transform.position, 0.1f);

            //Fire when cooldown is elapsed
            if (!firingCooldown) drone.StartCoroutine(Fire());
        }
        else
        {

        }
    }

    private IEnumerator Fire()
    {
        float orbitDamageMul = 1f;

        if (drone.orbit != null)
        {
            orbitDamageMul = drone.orbit.orbitData.damageMultiplier;
        }

        //Set cooldown active
        firingCooldown = true;

        //Set firing particles
        if (drone.muzzleFlashObject != null) { drone.muzzleFlashObject.SetActive(true); }

        //Apply damage to target
        currentFireTarget.DecreaseHealth((drone.droneActualData.GetCurrentLevelData().damagePerHit *
                                          drone.ammoData.damageMultiplier) * orbitDamageMul);

        //Notify hit
        currentFireTarget.Notify(new EnemyHitNotificationEventArgs(currentFireTarget, drone.gameObject));

        //Wait for fire rate
        yield return new WaitForSeconds(drone.droneActualData.GetCurrentLevelData().fireRate - 0.2f);

        //Reset firing particles
        if (drone.muzzleFlashObject != null) { drone.muzzleFlashObject.SetActive(false); }

        //Reset cooldown
        firingCooldown = false;
    }
}