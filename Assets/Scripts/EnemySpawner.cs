using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance; 
    public GameObject enemyPrefab;
    public int poolSize = 10;
    private Queue<GameObject> enemyPool = new Queue<GameObject>();
    public List<Vector3> waypoints;

    private void Awake()
    {
        if(instance != null)
        {
            instance = null;
        }
        instance = this;
    }
    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, this.transform);
            enemy.SetActive(false);
            enemyPool.Enqueue(enemy);
        }
    }
    private void Update()
    {
        // ObjectPooling 테스트를 위한 임시 코드
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Enemy enemy = GetEnemy().GetComponent<Enemy>();
            enemy.Init();            
        }
    }
    public GameObject GetEnemy()
    {
        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefab, this.transform);
            return enemy;
        }
    }
    public void ReturnToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
