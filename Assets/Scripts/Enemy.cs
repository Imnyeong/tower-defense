using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int maxHp = 100;
    public int hp;
    public float moveSpeed = 2.0f;

    [Header("UI")]
    public SpriteRenderer bodySprite;

    private int waypointIndex = 0;
    public List<Vector3> waypoints;

    private Coroutine hitCoroutine = null;
    public void Init()
    {
        hp = maxHp;
        waypointIndex = 0;
        waypoints = EnemySpawner.instance.waypoints;
        gameObject.transform.position = waypoints[0];
    }
    private void OnDisable()
    {
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
            hitCoroutine = null;
        }
    }
    private void Update()
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
        if(hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);      
            hitCoroutine = null;
        }
        hitCoroutine = StartCoroutine(HitCoroutine());

        if (hp <= 0)
        {
            EnemySpawner.instance.ReturnToPool(gameObject);
        }
    }

    private IEnumerator HitCoroutine()
    {
        for(int i = 0; i < 4; i++)
        {
            bodySprite.color = i % 2 == 0 ? ColorData.Color_Default : ColorData.Color_Translucent;
            yield return new WaitForSeconds(0.2f);
        }
        bodySprite.color = ColorData.Color_Default;
    }
}