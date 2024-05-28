using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Records : MonoBehaviour
{
    private int nMuertos;
    public TMP_Text muertos1;
    public TMP_Text muertos2;
    public TMP_Text muertos3;

    private int puesto1;
    private int puesto2;
    private int puesto3;
    
    // Start is called before the first frame update
    void Start()
    {
        // This unlocks the cursor
        Cursor.lockState = CursorLockMode.None;
 
        // This locks the cursor
        
 
        // If you unlock the cursor, but its still invisible, try this:
        
        nMuertos = 0;
        puntuaciones();
        
        
    }

    // Update is called once per frame
    
    public void Salir()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void puntuaciones()
    {
        
            nMuertos = PlayerPrefs.GetInt("NRondas");
            Debug.Log("Numero Rondas:  " + nMuertos);
            int primero = PlayerPrefs.GetInt("PrimerPuesto");
            Debug.Log("Primero:  " + primero);
            int segundo = PlayerPrefs.GetInt("SegundoPuesto");
            Debug.Log("segundo:  " + segundo);
            int tercero = PlayerPrefs.GetInt("TercerPuesto");
            Debug.Log("tercero:  " + tercero);

            if (nMuertos > primero)
            {
                Debug.Log("Rondas mayor que primero");
                muertos1.text = nMuertos.ToString();
                PlayerPrefs.SetInt("PrimerPuesto", nMuertos);
                muertos2.text = segundo.ToString();
                muertos3.text = tercero.ToString();
            
            }else if (nMuertos > segundo){
                muertos1.text = primero.ToString();
                Debug.Log("Rondas mayor que sec");
                muertos2.text = nMuertos.ToString();
                PlayerPrefs.SetInt("SegundoPuesto", nMuertos);
                muertos3.text = tercero.ToString();
            
            }else if (nMuertos > tercero){
                muertos1.text = primero.ToString();
                muertos2.text = segundo.ToString();
                Debug.Log("Rondas mayor que terc");
                muertos3.text = nMuertos.ToString();
                PlayerPrefs.SetInt("TercerPuesto", nMuertos);
            }else{
                muertos1.text = primero.ToString();
                muertos2.text = segundo.ToString();
                muertos3.text = tercero.ToString();
            }
        
    }
}

