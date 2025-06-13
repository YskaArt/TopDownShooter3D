using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public abstract class EnemyBase : MonoBehaviour
{
    [SerializeField] private ParticleSystem explosionParticle;
    [SerializeField] private GameObject brokenPrefab;
    [SerializeField] private float health = 100f;
    protected NavMeshAgent agent;
    protected Transform player;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public virtual void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            
            Die();
        }
    }

    protected virtual void Die()
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 0.5f;
        Instantiate(explosionParticle, spawnPosition, Quaternion.identity);
        Instantiate(brokenPrefab, spawnPosition, transform.rotation);
     
        Destroy(gameObject);
    }
}
