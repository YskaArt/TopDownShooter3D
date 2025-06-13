using UnityEngine;

public class HealingPickup : MonoBehaviour
{
    [SerializeField] private int healAmount = 20; 
    [SerializeField] private float rotationSpeed = 50f; 

    private void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth health = other.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.Heal(healAmount);
                Destroy(gameObject);
            }
        }
    }
}
