using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform door; // La puerta que queremos abrir/cerrar
    public float closedAngle = 90f; // Ángulo de la puerta cerrada (en grados, sobre el eje Y)
    public float openAngle = 14f; // Ángulo de la puerta abierta (en grados, sobre el eje Y)
    public float rotationSpeed = 2f; // Velocidad de la rotación de la puerta

    private bool isPlayerInside = false; // Verifica si el jugador está dentro del Trigger

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Asegúrate de que el objeto tiene la etiqueta "Player"
        {
            isPlayerInside = true;
            StopAllCoroutines(); // Detenemos cualquier animación previa
            StartCoroutine(RotateDoor(closedAngle)); // Comienza a cerrar la puerta
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Verifica si es el jugador saliendo del Trigger
        {
            isPlayerInside = false;
            StopAllCoroutines(); // Detenemos cualquier animación previa
            StartCoroutine(RotateDoor(openAngle)); // Comienza a abrir la puerta
        }
    }

    IEnumerator RotateDoor(float targetAngle)
    {
        Quaternion initialRotation = door.localRotation;
        Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
        float timeElapsed = 0f;

        while (timeElapsed < 1f)
        {
            door.localRotation = Quaternion.Lerp(initialRotation, targetRotation, timeElapsed);
            timeElapsed += Time.deltaTime * rotationSpeed; // Incrementar según la velocidad de rotación
            yield return null;
        }

        door.localRotation = targetRotation; // Asegurarse de que termine exactamente en el ángulo deseado
    }
}
