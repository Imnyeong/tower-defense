using System.Collections;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Status")]
    public int attackPower = 30;
    public float attackRange = 3f;
    public float trackDelay = 0.1f;
    public float attackDelay = 1f;

    private float attackTimer = 0f;

    [Header("Enemy")]
    public LayerMask enemyLayer;
    private Enemy target;

    [Header("UI")]
    public SpriteRenderer bodySprite;
    public SpriteRenderer hairSprite;
    public SpriteRenderer weaponSprite;
    public Animator animator;

    private Coroutine attackCoroutine = null;
    
    private void OnEnable()
    {
        attackCoroutine = StartCoroutine(AttackCoroutine());
    }
    private void OnDisable()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
        }
    }

    private IEnumerator AttackCoroutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(trackDelay);

            if (attackTimer >= attackDelay)
            {
                target = FindTarget();
                if (target != null)
                {
                    animator.Play(StringData.TowerAnimation_Attack);
                    attackTimer = 0f;
                }
            }
            else
            {
                attackTimer += trackDelay;
            }
        }
    }

    private Enemy FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        Enemy nearestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Enemy currentEnemy = hit.GetComponent<Enemy>();
            if (currentEnemy != null)
            {
                float distance = Vector3.Distance(transform.position, currentEnemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    nearestEnemy = currentEnemy;
                }
            }
        }
        if(nearestEnemy != null)
        {
            SetDirection(nearestEnemy.transform.position.x < transform.position.x);
        }
        return nearestEnemy;
    }

    private void SetDirection(bool isLeft)
    {
        bodySprite.flipX = isLeft;
        hairSprite.flipX = isLeft;
        weaponSprite.flipX = isLeft;
    }
    public void DoAttack()
    {
        if(target == null || !target.gameObject.activeInHierarchy)
        {
            Debug.Log("Å¸°Ù ¾øÀ½");
            return;
        }
        target.TakeDamage(attackPower);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
