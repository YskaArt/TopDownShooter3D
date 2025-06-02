using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Range(0f, 10f)]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private CharacterController characterController;

    [Header("Gravity")]
    [SerializeField] private float gravityForce = -9.81f;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;

    private float verticalVelocity;
    private Vector2 movementInput;
    private Vector2 mousePosition;
    private Vector3 lookTarget;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
        // Movimiento con gravedad
        if (!characterController.isGrounded)
        {
            verticalVelocity += gravityForce * Time.deltaTime;
        }
        else
        {
            verticalVelocity = 0f;
        }

        float movementY = (movementInput.y * movementSpeed * Time.deltaTime);
        float movementX = (movementInput.x * movementSpeed * Time.deltaTime);
        Vector3 movement = new Vector3(movementX, verticalVelocity, movementY);
        characterController.Move(movement);

        
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.down, transform.position);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        if (groundPlane.Raycast(ray, out float enter))
        {
            lookTarget = ray.GetPoint(enter);
            Vector3 direction = lookTarget - transform.position;

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 0.2f);
            }
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnMouseLook(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = firePoint.forward * bulletSpeed;
            }
        }
    }
}
