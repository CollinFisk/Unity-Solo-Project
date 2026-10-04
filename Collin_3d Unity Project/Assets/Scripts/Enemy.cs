using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isFollowing = false;

    public NavMeshAgent agent;
    public PlayerController player;
    public float detectionRadius = 5f;
    public float enemyHealth = 5f;
    public float expTime = 0.3f;

    public Transform enemy;
    public Transform EnemyAttackHitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        Explosion = GameObject.Find("Explosion");

    }

    // Update is called once per frame
    void Update()
    {
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
            detectionRadius = 10f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = false;
            detectionRadius = 5f;
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
                ContactPoint contact = other.GetContact(0);

                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                Instantiate(Explosion, pos, rot);
                Destroy(Explosion.gameObject, expTime);
        }
        if (other.gameObject.tag == "Explosion")
        {
            enemyHealth--;
        }
    }
}