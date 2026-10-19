using System.Collections;
using UnityEngine;

public class Boss1test : Enemy
{

    public float summonTime = 5f;

    public bool canSummon = true;

    public GameObject bossDrop;
    public GameObject summonRangedEnemy;
    public GameObject summonEnemy;

    void Update()
    {
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
