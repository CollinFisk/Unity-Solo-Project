using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Animator myAnim;

    public bool isFollowing = false;

    public NavMeshAgent agent;
    public PlayerController player;
    public float detectionRadius;
    public float enemyHealth;
    public float expTime = 0.3f;

    public Transform enemy;
    public Transform EnemyAttackHitbox;

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

    // Update is called once per frame
    public void Update()
    {
        if (enemyHealth <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().enemyCount--;
            isFollowing = false;
            Destroy(gameObject);
            if(player.enemyDamage)
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

        // temporary if statement for testing
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
        }
    }

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
}