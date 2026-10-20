using System.Collections;
using UnityEngine;

public class Boss1test : Enemy
{

    public float summonTime = 5f;

    public bool canSummon = true;

    public GameObject bossDrop;
    public GameObject summonRangedEnemy;
    public GameObject summonEnemy;

    
    public new void Update()
    {
        if (enemyHealth <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().enemyCount--;
            isFollowing = false;
            Destroy(gameObject);
            if (player.enemyDamage)
            {
                player.StopCoroutine("enemyDmgCooldown");
            }
        }

        EnemyAttackHitbox.position = enemy.position;
        EnemyAttackHitbox.rotation = enemy.rotation;

        GetComponent<SphereCollider>().radius = detectionRadius;
        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }

        if (enemyHealth <= 75 && canSummon)
        {
            StartCoroutine("Summons");
        }

        else if (enemyHealth <= 50 && enemyHealth >= 25)
        {
            summonTime = 4f;
        }

        else if (enemyHealth <= 25)
        {
            summonTime = 3f;
        }
    }
    IEnumerator Summons()
    {
        canSummon = false;

        yield return new WaitForSeconds(summonTime);

        if (enemyHealth <= 75)
        {
            GameObject p = Instantiate(summonEnemy, transform.position, transform.rotation);
        }

        if (enemyHealth <= 50)
        {
            GameObject p = Instantiate(summonRangedEnemy, transform.position, transform.rotation);
        }

        if (enemyHealth <= 25)
        {
            GameObject p = Instantiate(summonEnemy, transform.position, transform.rotation);
        }

        canSummon = true;
    }
}
