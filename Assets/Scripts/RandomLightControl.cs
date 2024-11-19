using System.Collections;
using UnityEngine;

public class RandomLightControl : MonoBehaviour
{
    public Light[] lights; // Array de luces
    public GameObject[] objectsToHide; // Array de objetos asociados a las luces
    public float minTime = 1f; // Tiempo mínimo de espera entre cambios (en segundos)
    public float maxTime = 5f; // Tiempo máximo de espera entre cambios (en segundos)

    private void Start()
    {
        // Llamar a la función que controla las luces aleatorias
        StartCoroutine(ToggleLightsRandomly());
    }

    private IEnumerator ToggleLightsRandomly()
    {
        while (true)
        {
            // Elegir una luz aleatoria del array
            int randomIndex = Random.Range(0, lights.Length);
            Light randomLight = lights[randomIndex];
            GameObject associatedObject = objectsToHide[randomIndex];

            // Cambiar el estado de la luz (prender o apagar)
            randomLight.enabled = !randomLight.enabled;

            // Si la luz está apagada, hacer invisible el objeto asociado
            if (!randomLight.enabled)
            {
                associatedObject.SetActive(false);
            }
            else
            {
                // Si la luz está encendida, hacer visible el objeto asociado
                associatedObject.SetActive(true);
            }

            // Esperar un tiempo aleatorio antes de cambiar la luz nuevamente
            float waitTime = Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);
        }
    }
}
