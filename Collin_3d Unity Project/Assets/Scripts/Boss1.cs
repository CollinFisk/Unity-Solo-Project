using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Boss1: MonoBehaviour
{
    Animator myAnim;

    public bool isFollowing = false;
    public bool attackingAnim;

    public NavMeshAgent agent;
    public PlayerController player;
    public float detectionRadius;
    public float enemyHealth;
    public float expTime = 0.3f;
    public float summonYLevel;

    public Transform enemy;
    public Transform EnemyAttackHitbox;

    public float summonTime = 5f;

    public bool canSummon = true;

    public GameObject bossDrop;
    public GameObject summonRangedEnemy;
    public GameObject summonEnemy;
  
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

        if (myAnim != null)
        {
            if (agent.velocity != Vector3.zero)
            {
                myAnim.SetBool("isMoving", true);
            }
            else
            {
                myAnim.SetBool("isMoving", false);
            }
            if (attackingAnim)
            {
                myAnim.SetBool("isMoving", false);
                myAnim.SetBool("Attack", true);
                attackingAnim = false;
            }
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        enemy = transform;
        EnemyAttackHitbox = enemy.GetChild(0);

        //temporary if statement
        if (transform.GetComponent<Animator>() != null)
        {
            myAnim = GetComponent<Animator>();
        }
    }

        // temporary if statement for testing


    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = false;
        }
    }

    public GameObject Explosion;

    public void OnCollisionEnter(Collision other)
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
        Vector3 summonPos = transform.position;
        summonPos.x = transform.position.x;
        summonPos.y = summonYLevel;
        summonPos.z = transform.position.z;
        canSummon = false;

        yield return new WaitForSeconds(summonTime);

        if (enemyHealth <= 75)
        {
            GameObject p = Instantiate(summonEnemy, summonPos, transform.rotation);
        }

        if (enemyHealth <= 50)
        {
            GameObject p = Instantiate(summonRangedEnemy, summonPos, transform.rotation);
        }

        if (enemyHealth <= 25)
        {
            GameObject p = Instantiate(summonEnemy, summonPos, transform.rotation);
        }

        canSummon = true;
    }
}