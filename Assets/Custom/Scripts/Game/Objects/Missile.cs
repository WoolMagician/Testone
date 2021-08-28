using UnityEngine;

public class Missile : MonoBehaviour
{
    public MissileData missileData;
    public Transform target;
    public Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        //transform.LookAt(target);
    }

    void FixedUpdate()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;
        direction.Normalize();

        rigidBody.angularVelocity = -(missileData.speed * 3) * Vector3.Cross(direction, transform.forward);
        rigidBody.velocity = transform.forward * missileData.speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collisionObj = other.gameObject;

        if (InputEventManager.Instance.enemyMask.Contains(collisionObj.layer))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.DecreaseHealth(missileData.damagePerHit);
            Destroy(this.gameObject);
        }
    }
}
