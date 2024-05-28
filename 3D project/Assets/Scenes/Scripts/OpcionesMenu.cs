using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OpcionesMenu : MonoBehaviour
{
    private int vida;
    public Slider sliderVolumen;
    public float volumInicial;
    private AudioSource musica;
    // Start is called before the first frame update
    void Start()
    {
        volumInicial = 0.5f;
        GameObject objeto1 = GameObject.Find("Los Muertos en la Calle");
        musica = objeto1.GetComponent<AudioSource>();
        vida = 3;
        PlayerPrefs.SetInt("Vida", vida);
        
        if (PlayerPrefs.HasKey("Volumen"))
        {
            float volumenGuardado = PlayerPrefs.GetFloat("Volumen");
            setVolumen(volumenGuardado);
        }
        else
        {
            
            setVolumen(volumInicial);
        }
        sliderVolumen.value = musica.volume;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HandleInputData(int val)
    {
        if(val == 0) {
             
             Debug.Log("Numero de vidas igual a 3");
             vida = 3;
             PlayerPrefs.SetInt("Vida", 3);
        }else if(val == 1) {
             
             Debug.Log("Numero de vidas igual a 5");
             vida = 5;
             PlayerPrefs.SetInt("Vida", 5);
        }else if(val == 2) {
             
             Debug.Log("Numero de vidas igual a 7");
             vida = 7;
             PlayerPrefs.SetInt("Vida", 7);
        }
    }

    public void setVolumen(float volumen)
    {
        // Ajustar el volumen del AudioSource de la música
        musica.volume = volumen;

        // Guardar el volumen en PlayerPrefs
        PlayerPrefs.SetFloat("Volumen", volumen);
        PlayerPrefs.Save(); // Guardar los cambios
    }

    public void irMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
