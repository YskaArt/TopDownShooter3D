using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [Header("Combat")]
    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private float fireCooldown = 1.5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    private EnemyState currentState = EnemyState.Pursue;
    private float nextFireTime = 0f;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > shootingRange)
        {
            currentState = EnemyState.Pursue;
        }
        else
        {
            currentState = EnemyState.Shoot;
        }

        switch (currentState)
        {
            case EnemyState.Pursue:
                agent.SetDestination(player.position);
                break;
            case EnemyState.Shoot:
                agent.SetDestination(transform.position);
                FaceTarget();
                TryShoot();
                break;
        }
    }

    private void FaceTarget()
    {
        Vector3 dir = (player.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
    }

    private void TryShoot()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + fireCooldown;

            if (bulletPrefab != null && firePoint != null)
            {
                GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
                Rigidbody rb = bullet.GetComponent<Rigidbody>();
                rb.linearVelocity = firePoint.forward * 20f; // o configurable
            }
        }
    }
}
