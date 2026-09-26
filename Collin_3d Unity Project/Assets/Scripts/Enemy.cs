using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public bool isFollowing = false;

    public NavMeshAgent agent;
    public PlayerController player;
    public float detectionRadius = 5f;

    public Transform enemy;
    public Transform EnemyAttackHitbox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

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
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
  
            // In update, make enemy attack and do damage to player while attacking
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // Stop attacking
        }
    }
    */
}