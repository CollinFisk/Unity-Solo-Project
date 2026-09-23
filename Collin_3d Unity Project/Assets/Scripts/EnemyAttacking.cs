using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class EnemyAttacking : MonoBehaviour
{
    public PlayerController player;

    public Transform EnemyAttackHitbox;
    public Transform enemy;

    public GameObject EnemyAttackSlash;


    public NavMeshAgent agent;

    public float SlashSpeed;
    public float SlashTime;

    public bool enemyAttacking;
    public bool isFollowing;

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
    }
    public void EnemyAttack(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttackHitbox")
        {
            StartCoroutine("Slash");
        }
    }
    IEnumerator Slash()
    {
        enemyAttacking = true;
        yield return new WaitForSeconds(SlashSpeed);

        isFollowing= false;
        EnemyAttackSlash.
        yield return new WaitForSeconds(SlashTime);
        Destroy(EnemyAttackSlash.gameObject);
        enemyAttacking = false;
        isFollowing = true;

        StopCoroutine("Slash");
    }
}
