using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss1
    : MonoBehaviour
{
    public bool isFollowing = false;

    public NavMeshAgent agent;
    public PlayerController player;
    public float detectionRadius = 8f;
    public float enemyHealth = 100f;
    public float expTime = 0.3f;
    public float summonTime = 5f;

    public bool canSummon = true;

    public GameObject bossDrop;
    public GameObject summonRangedEnemy;
    public GameObject summonEnemy;
    public Transform enemy;
    public Transform EnemyAttackHitbox;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemy = transform;
        EnemyAttackHitbox = enemy.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().enemyCount--;
            isFollowing = false;
            Destroy(gameObject);
            GameObject p = Instantiate(bossDrop, transform.position, transform.rotation);
            p.GetComponent<Rigidbody>();
            StopCoroutine("Summons");
            if (player.enemyDamage)
            {
                player.StopCoroutine("enemyDmgCooldown");
            }
        }
        else if (enemyHealth <= 75 && canSummon)
        {
            StartCoroutine ("Summons");
        }

        else if (enemyHealth <= 50 && enemyHealth >= 25)
        {
            summonTime = 4f;
        }

        else if (enemyHealth <= 25)
        {
            summonTime = 3f;
        }

        EnemyAttackHitbox.position = enemy.position;
        EnemyAttackHitbox.rotation = enemy.rotation;

        GetComponent<SphereCollider>().radius = detectionRadius;
        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = true;
            detectionRadius = 12f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = false;
            detectionRadius = 8f;
        }
    }

    public GameObject Explosion;

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);
            enemyHealth--;
        }

        if (other.gameObject.tag == "explodingProjectile")
        {
            Destroy(other.gameObject);
            enemyHealth--;
            GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
            Destroy(p, expTime);
        }
        if (other.gameObject.tag == "Explosion")
        {
            enemyHealth--;
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