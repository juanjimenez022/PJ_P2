using UnityEngine;

public class ZombieData
{
    public int strong;
    public float health;
    public float timeAlive;
    public float closestDistanceToPlayer;

    public ZombieData(int strong, float health, float timeAlive, float closestDistanceToPlayer)
    {
        this.strong = strong;
        this.health = health;
        this.timeAlive = timeAlive;
        this.closestDistanceToPlayer = closestDistanceToPlayer;
    }
}
