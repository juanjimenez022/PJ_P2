using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public Image barraVida;
    private float vidaActual = 1000;
    private float vidaMaxima = 1000;
    private float regenTimer = 0f;
    private float regenInterval = 5f;
    private float regenAmount = 100f;
    public RoundManager rm;
    

    void Update()
    {
        //Debug.Log("Puesto: " + PlayerPrefs.GetInt("PrimerPuesto"));

        if (vidaActual > 0)
        {
            regenTimer += Time.deltaTime;

            if (regenTimer >= regenInterval)
            {
                vidaActual = Mathf.Min(vidaActual + regenAmount, vidaMaxima);
                regenTimer = 0f;
            }
            barraVida.fillAmount = vidaActual / vidaMaxima;
        }
    }
    public void TakeDamage(int damage)
    {
        
        vidaActual -= damage;
        regenTimer = 0f;
        if (vidaActual <= 0)
        {
            //terminar el juego
            
            PlayerPrefs.SetInt("NRondas", rm.GetRound());
            //Debug.Log("Numero Rondas" + rm.GetRound());
            SceneManager.LoadScene("Records");


        }
        
    }

}
