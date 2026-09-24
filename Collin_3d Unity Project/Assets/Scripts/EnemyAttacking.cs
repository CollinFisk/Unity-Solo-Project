using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAttacking : MonoBehaviour
{
    public PlayerController player;

    public Transform enemy;

    public GameObject EnemyAttackSlash;
    public Transform EnemyAttackHitbox;

    
    public NavMeshAgent agent;

    public float SlashSpeed;
    public float SlashTime;

    public bool enemyAttacking = false;
    public bool isFollowing = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemy = GameObject.Find("Enemy").transform;
        EnemyAttackHitbox = GameObject.Find("EnemyAttackHitbox").transform;
    }

    // Update is called once per frame
        void Update()
        {
            EnemyAttackHitbox.position = enemy.position;
            EnemyAttackHitbox.rotation = enemy.rotation;

            if (enemyAttacking)
            {
                isFollowing = false;
            }
            else
            {
                isFollowing = true;
            }
        }
    
            public void StartEnemyAttack(Collider other)
            {
                if (other.gameObject.tag == "EnemyAttackHitbox")
                {
                    if (!enemyAttacking)
                    {
                     StartCoroutine("Slash1");
                    }
                    if (enemyAttacking)
                    {
                     GameObject p = Instantiate(EnemyAttackSlash, EnemyAttackHitbox.position, EnemyAttackHitbox.rotation);
                     Destroy(p, SlashTime);
                     enemyAttacking = false;
                    }
                }
            }

    IEnumerator Slash1()
    {
        yield return new WaitForSeconds(SlashSpeed);
        enemyAttacking = true;
    }
}
