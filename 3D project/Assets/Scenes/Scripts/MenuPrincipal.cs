using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    

public class MenuPrincipal : MonoBehaviour
{
    public 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Map_v2");
    }

    public void opciones()
    {
        SceneManager.LoadScene("Opciones");
    }

    public void records()

    {
        SceneManager.LoadScene("Records");
    }    

    public void QuitGame()

    {
        Application.Quit();
    }
}
