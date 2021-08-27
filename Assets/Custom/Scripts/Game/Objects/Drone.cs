using BezierSolution;
using DG.Tweening;
using System.Collections;
using UnityEngine;

public class Drone : BezierWalkerWithSpeed
{
    public enum TurretState
    {
        Idle,
        Patrol,
        Wake,
        Fire
    }

    public GameObject muzzleFlashObject;

    public TurretState CurrentState = TurretState.Idle;
    public Enemy currentFireTarget = null;


    public int currentLevel = 0;
    public TurretData turretData;
    public AmmoData ammoData;

    private bool allowFire = true;
    private bool allowBoosterStop = true;
    private int speedRandomizer = 1;

    GameObject turretObject;
    GameObject turretTrailObject;

    private bool InitializeTurret()
    {
        try
        {
            Destroy(turretObject);
            Destroy(turretTrailObject);

            if(turretData != null)
            {
                turretObject = Instantiate(turretData.GetLevelData(currentLevel).turretObject, this.transform);
                turretObject.transform.localScale = turretData.GetLevelData(currentLevel).turretObjectScaleOverride;
                turretTrailObject = Instantiate(turretData.GetLevelData(currentLevel).turretTrail, this.transform);
                muzzleFlashObject = Instantiate(turretData.GetLevelData(currentLevel).turretMuzzleFlash, turretObject.transform);
            }
        }
        catch
        {
            Debug.LogWarning("Turret initialization failed.");
            return false;
        }
        return true;
    }

    public void SetOrbit(Orbit orbit)
    {
        this.spline = orbit.OrbitSpline;
    }

    void Start()
    {
        this.InitializeTurret();
        speedRandomizer = (Random.Range(0, 2) == 0) ? 1 : -1;
    }

    void Update()
    {
        if (spline == null) return;

        Execute(Time.deltaTime);

        if (allowBoosterStop)
        {
            this.speed = turretData.GetLevelData(currentLevel).maxSpeed * speedRandomizer;
        }

        if (Input.GetKeyDown(KeyCode.C) && allowBoosterStop)
        {
            StartCoroutine(StopBoosterForTime(2));
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.currentLevel += 1;
            this.InitializeTurret();
        }

        switch (CurrentState)
        {
            case TurretState.Idle:
                break;
            case TurretState.Patrol:
                //if(allowBoosterStop)
                //{
                //    walker.enabled = true;
                //}
                break;
            case TurretState.Wake:
                break;
            case TurretState.Fire:
                this.HandleFireState();
                break;
            default:
                break;
        }

    }

    private void HandleFireState()
    {
        if (currentFireTarget != null)
        {
            float distance;
            RaycastHit hit;

            if (Physics.Linecast(this.transform.position, currentFireTarget.transform.position, out hit))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    distance = Vector3.Distance(this.transform.position, hit.point);

                    if (distance > turretData.GetLevelData(currentLevel).patrolSphereRadius)
                    {
                        currentFireTarget = null;
                        return;
                    }
                }
            }
            transform.DOLookAt(currentFireTarget.transform.position, 0.1f);

            if (allowFire) {StartCoroutine(Fire());}
        }
        else
        {
            if (muzzleFlashObject != null) { muzzleFlashObject.SetActive(false); }
            CurrentState = TurretState.Patrol;
        }
    }

    float walkerBrakeSpeed = 0.5f;
    float walkerSpeedTweenDuration = 0.75f;
    float actualWalkerSpeed = 3f;
    float maxWalkerSpeed = 3f;
    Tween walkerSpeedTween;

    void OnUpdateWalkerSpeed()
    {
        this.speed = actualWalkerSpeed * speedRandomizer;
    }
    void OnUpdateWalkerSpeedEnd()
    {
        walkerSpeedTween = null;
    }
    public void Brake(bool value)
    {
        walkerSpeedTween = DOTween.To(() => actualWalkerSpeed,
            x => actualWalkerSpeed = x, value ? walkerBrakeSpeed : maxWalkerSpeed, walkerSpeedTweenDuration).OnUpdate(OnUpdateWalkerSpeed);
        walkerSpeedTween.OnComplete(OnUpdateWalkerSpeedEnd);
    }

    public IEnumerator StopBoosterForTime(float time)
    {
        allowBoosterStop = false;
        this.Brake(true);
        yield return new WaitForSeconds(time);
        this.Brake(false);
        allowBoosterStop = true;
    }

    private IEnumerator Fire()
    {
        allowFire = false;
        if (muzzleFlashObject != null) { muzzleFlashObject.SetActive(true); }
        currentFireTarget.DecreaseHealth(turretData.GetLevelData(currentLevel).damagePerHit * ammoData.damageMultiplier);
        yield return new WaitForSeconds(turretData.GetLevelData(currentLevel).fireRate);
        if (muzzleFlashObject != null) { muzzleFlashObject.SetActive(false); }
        allowFire = true;
    }


    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") && currentFireTarget == null)
        {
            currentFireTarget = other.gameObject.GetComponentInParent<Enemy>();
            CurrentState = TurretState.Fire;
        }
    }
}
