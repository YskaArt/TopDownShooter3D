using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private float fireCooldown = 0.5f;

    [Header("Crosshair")]
    [SerializeField] private Image crosshairImage;

    [Header("Tank Components")]
    [SerializeField] private Transform turretTransform; 

    private float nextFireTime = 0f;
    private float verticalVelocity;
    private Vector2 movementInput;
    private Vector2 mousePosition;
    private Vector3 lookTarget;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
    }

    private void Update()
    {
        HandleMovement();
        HandleTurretRotation();
        UpdateCrosshairPosition();
    }

    private void HandleMovement()
    {
        // Movimiento con gravedad
        if (!characterController.isGrounded)
            verticalVelocity += gravityForce * Time.deltaTime;
        else
            verticalVelocity = 0f;

        Vector3 move = new Vector3(movementInput.x, 0f, movementInput.y);

        // Si hay movimiento, rotamos el cuerpo hacia esa dirección
        if (move.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f); // rotación suave
        }

        move *= movementSpeed * Time.deltaTime;
        move.y = verticalVelocity;

        characterController.Move(move);
    }

    private void HandleTurretRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        Plane groundPlane = new Plane(Vector3.down, transform.position);

        if (groundPlane.Raycast(ray, out float enter))
        {
            lookTarget = ray.GetPoint(enter);
            Vector3 dirToTarget = lookTarget - turretTransform.position;
            dirToTarget.y = 0f;

            if (dirToTarget.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(dirToTarget);
                turretTransform.rotation = Quaternion.Slerp(turretTransform.rotation, targetRotation, 0.2f);
            }
        }
    }

    private void UpdateCrosshairPosition()
    {
        if (crosshairImage != null)
        {
            crosshairImage.rectTransform.position = mousePosition;
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
        if (context.performed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireCooldown;
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
