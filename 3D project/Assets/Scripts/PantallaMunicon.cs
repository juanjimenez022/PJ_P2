using UnityEngine;
using UnityEngine.UI;

public class PantallaMunicon : MonoBehaviour
{
    public Text texto;
    public Escopeta2Canones escopeta;
    public PPSH41 ppsh;
    public RoundManager RM;
    
    // Update is called once per frame
    void Update()
    {
        texto.text = "Munici�n de Escopeta: " + escopeta.GetAmmoLoad() + " / \u221E" + "\n" + 
                     "Munici�n del PPSH-41: " + ppsh.GetAmmoLoad() + " / \u221E" + "\n" +
                     "Enemigos: " + RM.GetEnemies() + "\n" +
                     "Ronda: " + RM.GetRound() + "\n";
    }
}
