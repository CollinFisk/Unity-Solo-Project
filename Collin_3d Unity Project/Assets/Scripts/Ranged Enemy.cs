using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemies : MonoBehaviour
{
    public bool isFollowing = false;
    public bool enemyCanFire = true;
    public bool efireCooldown = true;


    public NavMeshAgent agent;
    public PlayerController player;
    public Vector2 efiringDirection;
   
    public float detectionRadius = 20f;
    public float enemyHealth = 5f;
    public float expTime = 0.3f;

    [Header("Object Refrences")]
    public GameObject eprojectile;
    public Transform efirePoint;
    public Transform RangedEnemy;
    public Transform eweaponSlot;
    public Transform EnemyGun;

    [Header("Weapon Stats")]
    public float eprojLifespan;
    public float eprojVelocity;
    public float erof;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        eweaponSlot = RangedEnemy.GetChild(0);
        efirePoint = EnemyGun.GetChild(0);
        RangedEnemy = GameObject.FindWithTag("RangedEnemy").transform;
        efiringDirection = RangedEnemy.forward;

        EnemyGun.SetPositionAndRotation(eweaponSlot.position, eweaponSlot.rotation);
        EnemyGun.SetParent(eweaponSlot);
        EnemyGun.GetComponent<Rigidbody>().isKinematic = true;
        EnemyGun.GetComponent<Collider>().isTrigger = true;

        Explosion = GameObject.Find("Explosion");

    }


    // Update is called once per frame
    void Update()
    {
        if (enemyHealth <= 0)
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().enemyCount--;
            isFollowing = false;
            Destroy(gameObject);
        }

        GetComponent<SphereCollider>().radius = detectionRadius;
        if (isFollowing)
        {
            agent.destination = player.transform.position;
        }

        var pos = player.transform.position;
        RangedEnemy.LookAt(pos);

        Vector3 euler = RangedEnemy.rotation.eulerAngles;
        RangedEnemy.rotation = Quaternion.Euler(0, euler.y, 0);



        if (isFollowing && enemyCanFire)
        {

            GameObject p = Instantiate(eprojectile, efirePoint.position, efirePoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(RangedEnemy.transform.forward * eprojVelocity);
            Destroy(p, eprojLifespan);
            efireCooldown = true;
            enemyCanFire = false;
        }
}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = true;
            detectionRadius = 20f;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = true;
            if (!enemyCanFire && efireCooldown)
            {
                StartCoroutine("ecooldownFire");
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = false;
            StopCoroutine("ecooldownFire");
            enemyCanFire = false;
            detectionRadius = 15f;
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

    IEnumerator ecooldownFire()
    {
        efireCooldown = false;
        yield return new WaitForSeconds(erof);
        enemyCanFire = true;
    }
}