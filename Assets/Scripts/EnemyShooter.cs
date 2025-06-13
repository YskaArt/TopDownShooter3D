using UnityEngine;

public class EnemyShooter : EnemyBase
{
    [Header("Combat")]
    [SerializeField] private float shootingRange = 10f;
    [SerializeField] private float fireCooldown = 1.5f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Tank Components")]
    [SerializeField] private Transform turretTransform; 
   
    private EnemyState currentState = EnemyState.Pursue;
    private float nextFireTime = 0f;

    private void Update()
    {
  
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;

                if (player == null)
                {
                    
                    return;
                }
            }

            
            Vector3 direction = player.transform.position - transform.position;
           
        

        float distance = Vector3.Distance(transform.position, player.position);

        // Cambio de estado según rango
        currentState = (distance > shootingRange) ? EnemyState.Pursue : EnemyState.Shoot;

        switch (currentState)
        {
            case EnemyState.Pursue:
                agent.SetDestination(player.position);
                RotateBodyToMovement(); //  El cuerpo gira hacia el destino
                break;

            case EnemyState.Shoot:
                agent.SetDestination(transform.position); // se queda quieto
                RotateTurretToPlayer();                    // torreta apunta al jugador
                TryShoot();
                break;
        }
    }

    private void RotateBodyToMovement()
    {
        Vector3 velocity = agent.desiredVelocity;

        if (velocity.sqrMagnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
        }
    }

    private void RotateTurretToPlayer()
    {
        Vector3 dirToPlayer = player.position - turretTransform.position;
        dirToPlayer.y = 0f;

        if (dirToPlayer.sqrMagnitude > 0.01f)
        {
            Quaternion turretRot = Quaternion.LookRotation(dirToPlayer);
            turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, turretRot, Time.deltaTime * 8f);
        }
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
                if (rb != null)
                    rb.linearVelocity = firePoint.forward * 20f; // velocidad configurable
            }
        }
    }
}
