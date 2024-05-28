using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoundManager : MonoBehaviour
{
    private static RoundManager _instance;
    public static RoundManager Instance { get { return _instance; } }

    private List<ZombieData> deadZombiesData = new List<ZombieData>();

    private int currentRound = 1, EnemyCounts = 0;
    public Respawn respawnPoint1;
    public Respawn respawnPoint2;

    private int nextRoundStrong;
    private float nextRoundHealth;

    private float gameTime = 0f;  // Variable para rastrear el tiempo de juego

    private bool isWaitingForNextRound = false;  // Bandera para evitar iniciar múltiples corrutinas

    public int ZombiesMuertos = 0;

    public void ZombieMuerto()
    {
        ZombiesMuertos++;
    }
    public int GetRound()
    {
        return currentRound;
    }

    public int GetEnemies()
    {
        return EnemyCounts;
    }

    private void Awake()
    {
        _instance = this;
    }

    public void AddDeadZombieData(ZombieData zombieData)
    {
        deadZombiesData.Add(zombieData);
    }

    private void Start()
    {
        respawnPoint1.Ronda1(3);
        respawnPoint2.Ronda1(3);
    }

    private void Update()
    {
        gameTime += Time.deltaTime;  // Actualizar el tiempo de juego
        EnemyCounts = CountZombies();
        if (EnemyCounts == 0 && gameTime > 10f && !isWaitingForNextRound)  // Comprobar que no haya zombies y que el tiempo de juego sea mayor a 5 segundos
        {
            isWaitingForNextRound = true;
            gameTime = 0f;
            StartCoroutine(HandleRoundTransition());
        }
    }

    private int CountZombies()
    {
        CapsuleCollider[] zombies = Object.FindObjectsOfType<CapsuleCollider>();
        int count = 0;

        foreach (CapsuleCollider collider in zombies)
        {
            if (collider.GetComponent<ZombieAI>() != null)
            {
                count++;
            }
        }

        return count;
    }

    private void IncrementRound()
    {
        currentRound++;
        //Debug.Log("Round: " + currentRound);
    }

    private void StartNewRound()
    {
        respawnPoint1.Ronda1(2);
        respawnPoint2.Ronda1(2);
        int zombiesToSpawn = (currentRound * 5) / 2;
        respawnPoint1.SetGenetic(nextRoundStrong, nextRoundHealth);
        respawnPoint1.CreaMasZombies(zombiesToSpawn);

        respawnPoint2.SetGenetic(nextRoundStrong, nextRoundHealth);
        respawnPoint2.CreaMasZombies(zombiesToSpawn);

        deadZombiesData.Clear();
    }

    private void EvaluateZombies()
    {
        if (deadZombiesData.Count > 0)
        {
            float maxTimeAlive = 0f;
            float minDistanceToPlayer = float.MaxValue;
            ZombieData bestTimeZombie = null;
            ZombieData bestDistanceZombie = null;

            foreach (ZombieData zombieData in deadZombiesData)
            {
                if (zombieData.timeAlive > maxTimeAlive)
                {
                    maxTimeAlive = zombieData.timeAlive;
                    bestTimeZombie = zombieData;
                }

                if (zombieData.closestDistanceToPlayer < minDistanceToPlayer)
                {
                    minDistanceToPlayer = zombieData.closestDistanceToPlayer;
                    bestDistanceZombie = zombieData;
                }
            }

            if (bestTimeZombie != null)
            {
                nextRoundHealth = bestTimeZombie.health;
            }

            if (bestDistanceZombie != null)
            {
                nextRoundStrong = bestDistanceZombie.strong;
            }

            Debug.Log("Evaluación terminada...");
            Debug.Log("Vida Genetica: " + nextRoundHealth);
            Debug.Log("Fuerza Genetica:" + nextRoundStrong);
        }
    }

    private IEnumerator HandleRoundTransition()
    {
        EvaluateZombies();
        IncrementRound();

        yield return new WaitForSeconds(5f);  // Tiempo de descanso entre rondas

        StartNewRound();
        isWaitingForNextRound = false;
    }

}
