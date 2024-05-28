using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class PantallaInicio : MonoBehaviour
{
    public GameObject musica;
    private AudioSource audio;
    public GameObject brillo;
    AutoExposure exposure;
    public PostProcessProfile profbrillo;
    public PostProcessLayer layerBrillo;
    
    
    private float volumenPorDefecto;
    private float volumenGuardado;

    // Start is called before the first frame update
    void Start()
    {
        profbrillo.TryGetSettings(out exposure);
        DontDestroyOnLoad(musica);
        DontDestroyOnLoad(brillo);
        audio = musica.GetComponent<AudioSource>();
        if(PlayerPrefs.HasKey("Volumen"))
        {
            volumenGuardado = PlayerPrefs.GetFloat("Volumen");
            SetVolumenMusica(volumenGuardado);
        }else{
            SetVolumenMusica(volumenPorDefecto);
        }

        if(PlayerPrefs.HasKey("Brillo"))
        {
            float nuevoBrillo = PlayerPrefs.GetFloat("Brillo");
            AjustaBrillo(nuevoBrillo);
        }else{
            AjustaBrillo(0.5f);
            
        
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("BrilloSelect");
        }


        
    }

    public void SetVolumenMusica(float volumen)
    {
        audio.volume = volumen;
        PlayerPrefs.SetFloat("Volumen", volumen);
        PlayerPrefs.Save();
    }

    public void AjustaBrillo(float valor)
    {
        if(valor != 0)
        {
            exposure.keyValue.value = valor;
            PlayerPrefs.SetFloat("Brillo", valor);
            PlayerPrefs.Save();
        }
        else
        {
            exposure.keyValue.value = .05f;
            PlayerPrefs.SetFloat("Brillo", valor);
            PlayerPrefs.Save();
        }

    }
}
