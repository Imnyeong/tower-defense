using UnityEngine;
using System.Collections.Generic;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    private int waypointIndex = 0;
    public List<Vector3> waypoints;

    public void Init()
    {
        waypointIndex = 0;
        waypoints = EnemySpawner.instance.waypoints;
        gameObject.transform.localPosition = waypoints[0];
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
}