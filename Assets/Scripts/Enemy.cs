using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int maxHp = 100;
    public int hp;
    public float moveSpeed = 2.0f;

    private int waypointIndex = 0;
    public List<Vector3> waypoints;

    public void Init()
    {
        hp = maxHp;
        waypointIndex = 0;
        waypoints = EnemySpawner.instance.waypoints;
        gameObject.transform.position = waypoints[0];
    }
    void Update()
    {
        if (waypointIndex == waypoints.Count)
        {
            EnemySpawner.instance.ReturnToPool(gameObject);
        }
        if (waypointIndex < waypoints.Count)
        {
            transform.position = Vector3.MoveTowards(transform.position, waypoints[waypointIndex], moveSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, waypoints[waypointIndex]) < 0.1f)
            {
                waypointIndex++;
            }
        }
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            EnemySpawner.instance.ReturnToPool(gameObject);
        }
    }
}