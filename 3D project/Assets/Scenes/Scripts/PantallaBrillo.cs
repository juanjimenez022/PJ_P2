
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class PantallaBrillo : MonoBehaviour
{
    public PostProcessProfile profbrillo;
    public PostProcessLayer layerBrillo;
    public Slider brilloSlider;
    public GameObject objetoBrillo;
    AutoExposure exposure;
    public const string claveBrillo = "Brillo";

    // Start is called before the first frame update
    void Start()
    {
        profbrillo.TryGetSettings(out exposure);
        if(PlayerPrefs.HasKey("Brillo"))
        {
            float nuevoBrillo = PlayerPrefs.GetFloat("Brillo");
            AjustaBrillo(nuevoBrillo);
            brilloSlider.value = nuevoBrillo;
        }else{
            AjustaBrillo(0.5f);
            brilloSlider.value = 0.5f;
        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
       {
            SceneManager.LoadScene("MainMenu");
       } 
        
    }

    public void AjustaBrillo(float valor)
    {
        if(valor != 0)
        {
            exposure.keyValue.value = valor;
            PlayerPrefs.SetFloat(claveBrillo, valor);
            PlayerPrefs.Save();
        }
        else
        {
            exposure.keyValue.value = .05f;
            PlayerPrefs.SetFloat(claveBrillo, valor);
            PlayerPrefs.Save();
        }

    }
}
