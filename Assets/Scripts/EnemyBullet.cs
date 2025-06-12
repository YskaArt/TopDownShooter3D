using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
            Destroy(gameObject); // Destruir la bala tras el impacto
        }
        if (other.CompareTag("Obstaculo"))
        {
            Destroy(gameObject);
        }
    }
}
