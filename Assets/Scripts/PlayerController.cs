using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float velocidadMovimiento = 5.0f;
    public float alturaSalto = 5.0f;
    public float sensibilidadRaton = 2.0f;
    public float inclinacionMaxima = 45.0f;
    public float inclinacionMinima = -45.0f;
    public float gravedad = 20.0f;

    private CharacterController characterController;
    private Vector3 movimientoDireccion;
    private float velocidadY;
    private float rotacionX = 0;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ManejarMovimiento();
        ManejarSalto();
        RotarConMouse();
    }

    void ManejarMovimiento()
    {
        float inputHorizontal = Input.GetAxis("Horizontal");
        float inputVertical = Input.GetAxis("Vertical");

        movimientoDireccion = new Vector3(inputHorizontal, 0, inputVertical);
        movimientoDireccion.Normalize();

        movimientoDireccion = transform.TransformDirection(movimientoDireccion);

        movimientoDireccion *= velocidadMovimiento;

        // Aplicar gravedad
        velocidadY -= gravedad * Time.deltaTime;
        movimientoDireccion.y = velocidadY;

        // Mover al jugador
        characterController.Move(movimientoDireccion * Time.deltaTime);
    }

    void ManejarSalto()
    {
        if (characterController.isGrounded)
        {
            velocidadY = -0.5f; // Resetea la velocidad vertical cuando está en el suelo

            if (Input.GetButtonDown("Jump"))
            {
                velocidadY = Mathf.Sqrt(alturaSalto * -2f * gravedad);
            }
        }
    }

    void RotarConMouse()
    {
        float rotacionY = Input.GetAxis("Mouse X") * sensibilidadRaton;
        rotacionX -= Input.GetAxis("Mouse Y") * sensibilidadRaton;
        rotacionX = Mathf.Clamp(rotacionX, inclinacionMinima, inclinacionMaxima);

        transform.Rotate(0, rotacionY, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(rotacionX, 0, 0);
    }
}
