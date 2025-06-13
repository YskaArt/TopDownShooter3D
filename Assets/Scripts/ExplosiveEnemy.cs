using UnityEngine;

public class ExplosiveEnemy : EnemyBase
{
    [SerializeField] private int damage = 20; 

    private void Update()
    {
       
        if (player != null && agent.isActiveAndEnabled)
        {
            agent.SetDestination(player.position);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            
            PlayerHealth health = collision.transform.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(damage);
            }
           
            Die();
        }
    }
}
