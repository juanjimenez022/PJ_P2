using UnityEngine;
using System.Linq;

public class Respawn : MonoBehaviour
{
    private int strong; // Valor de strong para los zombies
    private float health; // Valor de health para los zombies
    private float minX, maxX, minY, maxY, minZ, maxZ;

    [SerializeField] private Transform[] puntos; //Indican en que zona que encierran los puntos pueden spawnear los zombies
    [SerializeField] private GameObject zombie;
    private float tiempoDeSpawn = 1f;

    private float tiempoSiguienteZombie;
    private int numeroTotalSpawnear = 0;

    public void SetGenetic(int strong, float health)
    {
        this.strong = strong;
        this.health = health;
    }

    private void Start()
    {
        maxX = puntos.Max(punto => punto.position.x);
        minX = puntos.Min(punto => punto.position.x);
        maxY = puntos.Max(punto => punto.position.y);
        minY = puntos.Min(punto => punto.position.y);
        maxZ = puntos.Max(punto => punto.position.z);
        minZ = puntos.Min(punto => punto.position.z);
    }

    private void Awake()
    {
        maxX = puntos.Max(punto => punto.position.x);
        minX = puntos.Min(punto => punto.position.x);
        maxY = puntos.Max(punto => punto.position.y);
        minY = puntos.Min(punto => punto.position.y);
        maxZ = puntos.Max(punto => punto.position.z);
        minZ = puntos.Min(punto => punto.position.z);
    }


    private void Update()
    {
        tiempoSiguienteZombie += Time.deltaTime;

        if (tiempoSiguienteZombie >= tiempoDeSpawn && numeroTotalSpawnear > 0)
        {
            numeroTotalSpawnear--;
            tiempoSiguienteZombie = 0f;
            CrearZombie();
        }
    }


    private void CrearZombie()
    {
        Vector3 posicionAleatoria = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minZ, maxZ));
        GameObject nuevoZombie = Instantiate(zombie, posicionAleatoria, Quaternion.identity);

        // Configurar atributos del zombie
        ZombieAI zombieAI = nuevoZombie.GetComponent<ZombieAI>();
        if (zombieAI != null)
        {
            zombieAI.SetGenetic(strong, health);
            //Debug.Log("Zombie generado con Fuerza: " + zombieAI.strong + " y Vida: "+ zombieAI.health);
        }
        else
        {
            Debug.Log("ZombieAI es null al crear el zombie en respawn");
        }
    }

    public void Ronda1(int NZ)
    {
        for (int i = 0; i < NZ; i++)
        {
            this.strong = (Random.Range(1, 11) * 10);
            this.health = ((Random.Range(10, 50)) * 10);
            CrearZombie();
        }
    }

    public void CreaMasZombies(int cantidad)
    {
        numeroTotalSpawnear = cantidad;
    }
}
