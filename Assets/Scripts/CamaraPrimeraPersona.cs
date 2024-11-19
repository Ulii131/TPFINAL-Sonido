using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraPrimeraPersona : MonoBehaviour
{
    public float velocidadRotacionSuave = 1.0f;

    void Update()
    {
        // Rotación suave alrededor del eje Y
        transform.Rotate(0, velocidadRotacionSuave * Time.deltaTime, 0);
    }
}
