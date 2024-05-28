using UnityEngine;
using UnityEngine.UI;

public class BarraVida : MonoBehaviour
{
    public Image barraVida;
    public float vidaActual;
    public float vidaMaxima;
    void Update()
    {
        if (vidaActual > 0)
        {
            barraVida.fillAmount = vidaActual / vidaMaxima;
        }
    }
}
