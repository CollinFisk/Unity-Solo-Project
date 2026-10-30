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
    Animator myAnim;

    public Vector2 efiringDirection;
   
    public float detectionRadius = 15f;
    public float enemyHealth = 5f;
    public float expTime = 0.3f;

    [Header("Object Refrences")]
    public GameObject Explosion;
    public GameObject eprojectile;
    public Transform efirePoint;
    public Transform RangedEnemy;
    public Transform eweaponSlot;
    public Transform EnemyGun;

    [Header("Weapon Stats")]
    public float eprojLifespan;
    public float eprojVelocity;
    public float erof;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        eweaponSlot = RangedEnemy.GetChild(0);
        EnemyGun = eweaponSlot.GetChild(0);
        efirePoint = EnemyGun.GetChild(0);
        RangedEnemy = transform;
        efiringDirection = RangedEnemy.forward;

        EnemyGun.SetPositionAndRotation(eweaponSlot.position, eweaponSlot.rotation);
        EnemyGun.GetComponent<Rigidbody>().isKinematic = true;
        EnemyGun.GetComponent<Collider>().isTrigger = true;

        if (transform.GetComponent<Animator>() != null)
        {
            myAnim = GetComponent<Animator>();
        }
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


            var pos = player.transform.position;

            EnemyGun.transform.LookAt(pos);
            transform.LookAt(pos);

            Vector3 gunEuler = EnemyGun.rotation.eulerAngles;
            EnemyGun.rotation = Quaternion.Euler(gunEuler.x + 90, gunEuler.y, gunEuler.z);

            Vector3 Euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, Euler.y, 0);
        }


        if (isFollowing && enemyCanFire)
        {
            myAnim.SetBool("isMoving", false);
            myAnim.SetBool("Attack", true);
         //   myAnim.SetBool("Attack", false);
            GameObject p = Instantiate(eprojectile, efirePoint.position, efirePoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(efirePoint.forward * eprojVelocity);
            Destroy(p, eprojLifespan);
            efireCooldown = true;
            enemyCanFire = false;
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            isFollowing = true;
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
        }
    }
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
        myAnim.SetBool("Attack", false);
        enemyCanFire = true;
    }
}