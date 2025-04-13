using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("Status")]
    public int attackPower = 30;
    public float attackRange = 3f;
    public float attackDelay = 1f;

    [Header("Enemy")]
    public LayerMask enemyLayer;
    private Enemy target;

    [Header("UI")]
    public SpriteRenderer bodySprite;
    public SpriteRenderer hairSprite;
    public SpriteRenderer weaponSprite;
    public Animator animator;
    
    private float attackTimer = 0f;
    void Update()
    {
        if (attackTimer >= attackDelay)
        {
            target = FindTarget();
            if (target != null)
            {
                animator.Play("Tower_Attack");
                attackTimer = 0f;
            }
        }
        else
        {
            attackTimer += Time.deltaTime;
        }
    }

    private Enemy FindTarget()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

        if (hits.Length == 0)
        {
            return null;
        }

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
        SetDirection(nearestEnemy.transform.position.x < transform.position.x);
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
        target.TakeDamage(attackPower);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
