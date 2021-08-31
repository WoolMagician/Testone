using BezierSolution;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DroneGenerateEnergyOvertime", menuName = "Scriptable Objects/Power UPs/DroneGenerateEnergyOvertime")]
public class DroneGenerateEnergyOvertimeSO : PowerUPSO
{
    [SerializeField]
    private DroneGenerateEnergyOvertime _powerup = new DroneGenerateEnergyOvertime();
    public override BasePowerUP PowerUP { get => new DroneGenerateEnergyOvertime() { energyQuantity = _powerup.energyQuantity, generationCooldown = _powerup.generationCooldown }; }
}

[System.Serializable]
public class DroneGenerateEnergyOvertime : BasePowerUP
{
    public int energyQuantity = 1;
    public float generationCooldown = 5f;

    [System.NonSerialized]
    private bool energyGenCooldown = false;

    public override void ApplyPowerUP(IData data, IHasPowerUPs poweredUpObject)
    {
        base.ApplyPowerUP(data, poweredUpObject);

        Drone referenceDrone = (Drone)poweredUpObject;
        BezierWalkerWithSpeed walker = referenceDrone.walker;

        // Null checks
        if (walker == null || walker.spline == null) return;

        // Execute walker
        walker.Execute(Time.deltaTime);

        if (!energyGenCooldown) ((Drone)poweredUpObject).StartCoroutine(GenerateEnergy());
    }

    private IEnumerator GenerateEnergy()
    {
        energyGenCooldown = true;
        yield return new WaitForSeconds(generationCooldown);
        GameManager.Instance.simulationData.mineralAcquired += energyQuantity;
        energyGenCooldown = false;
    }
}