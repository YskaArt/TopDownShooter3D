using UnityEngine;

public class TankBullet : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float lifeTime = 15f;
    [SerializeField] private GameObject impactEffect; // opcional

    private void Start()
    {
        Destroy(gameObject, lifeTime); 
    }

    private void OnTriggerEnter(Collider other)
    {
        // Si golpea un enemigo con EnemyBase
        EnemyBase enemy = other.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Explode();
            return;
        }
        if (other.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }


    }

    private void Explode()
    {
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
