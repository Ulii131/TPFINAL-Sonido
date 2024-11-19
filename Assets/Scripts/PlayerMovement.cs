using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Velocidad de movimiento
    public float mouseSensitivity = 100f; // Sensibilidad del mouse
    public float gravity = -9.81f; // Gravedad personalizada

    private CharacterController controller;
    private Transform cameraTransform;
    private Vector3 velocity;
    private bool isGrounded;
    public float groundCheckDistance = 0.4f; // Distancia para verificar el suelo
    public LayerMask groundMask; // Capa del suelo

    private float xRotation = 0f;
    private float smoothTime = 0.1f; // Tiempo de suavizado para el movimiento de la cámara

    // Variables para el bobbing
    public float bobbingSpeed = 14f; // Velocidad del efecto de bobbing
    public float bobbingAmount = 0.05f; // Amplitud del movimiento
    private float defaultCameraYPos; // Posición original de la cámara
    private float bobbingTimer = 0f;

    private Vector3 currentCameraRotation;
    private Vector3 currentCameraRotationVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("No se encontró la cámara principal. Asegúrate de etiquetar la cámara como 'MainCamera'.");
        }

        // Guardar la posición inicial de la cámara
        defaultCameraYPos = cameraTransform.localPosition.y;

        // Ocultar y bloquear el cursor en la pantalla
        Cursor.lockState = CursorLockMode.Locked;

        // Inicializar la rotación de la cámara
        currentCameraRotation = cameraTransform.eulerAngles;
    }

    void Update()
    {
        // Movimiento del mouse con interpolación suave
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Actualizar la rotación de la cámara en Y (horizontal) y en X (vertical)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicar suavizado a la rotación de la cámara
        currentCameraRotation.x = Mathf.SmoothDamp(currentCameraRotation.x, xRotation, ref currentCameraRotationVelocity.x, smoothTime);
        currentCameraRotation.y += mouseX;

        cameraTransform.localRotation = Quaternion.Euler(currentCameraRotation.x, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // Movimiento del jugador
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        // Verificar si está en el suelo
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Mantener al jugador pegado al suelo
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Efecto de bobbing
        HandleHeadBobbing();
    }

    private void HandleHeadBobbing()
    {
        if (controller.velocity.magnitude > 0.1f && isGrounded) // Si el jugador está caminando
        {
            bobbingTimer += Time.deltaTime * bobbingSpeed;
            float newY = Mathf.Sin(bobbingTimer) * bobbingAmount;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(cameraTransform.localPosition.x, defaultCameraYPos + newY, cameraTransform.localPosition.z), 0.1f);
        }
        else
        {
            // Resetear la posición de la cámara cuando el jugador no camina
            bobbingTimer = 0f;
            cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, new Vector3(cameraTransform.localPosition.x, defaultCameraYPos, cameraTransform.localPosition.z), 0.1f);
        }
    }
}
