using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class boss2 : MonoBehaviour
{
    public bool isFollowing = false;
    public bool enemyCanFire = true;
    public bool efireCooldown = true;


    public NavMeshAgent agent;
    public PlayerController player;

    public float detectionRadius = 15f;
    public float bossHealth = 100f;
    public float expTime = 0.3f;

    [Header("Object Refrences")]
    public GameObject Explosion;
    public GameObject eprojectile;
    public Transform efirePoint;
    public Transform efirePoint2;
    public Transform Boss2;
    public Transform eweaponSlot;
    public Transform eweaponSlot2;
    public Transform EnemyGun;
    public Transform EnemyGun2;

    [Header("Weapon Stats")]
    public float eprojLifespan;
    public float eprojVelocity;
    public float erof;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        eweaponSlot = Boss2.GetChild(0);
        eweaponSlot2 = Boss2.GetChild(1);
        EnemyGun = eweaponSlot.GetChild(0);
        EnemyGun2 = eweaponSlot2.GetChild(0);
        efirePoint = EnemyGun.GetChild(0);
        efirePoint2 = EnemyGun2.GetChild(0);
        Boss2 = transform;

        EnemyGun.SetPositionAndRotation(eweaponSlot.position, eweaponSlot.rotation);
        EnemyGun.GetComponent<Rigidbody>().isKinematic = true;
        EnemyGun.GetComponent<Collider>().isTrigger = true;

        EnemyGun2.SetPositionAndRotation(eweaponSlot2.position, eweaponSlot2.rotation);
        EnemyGun2.GetComponent<Rigidbody>().isKinematic = true;
        EnemyGun2.GetComponent<Collider>().isTrigger = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (bossHealth <= 0)
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
        EnemyGun.transform.LookAt(pos);
        EnemyGun2.transform.LookAt(pos);
        Boss2.transform.LookAt(pos);

        Vector3 gunEuler = EnemyGun.rotation.eulerAngles;
        EnemyGun.rotation = Quaternion.Euler(gunEuler.x + 90, gunEuler.y, gunEuler.z);

        Vector3 gunEuler2 = EnemyGun2.rotation.eulerAngles;
        EnemyGun2.rotation = Quaternion.Euler(gunEuler.x + 90, gunEuler2.y, gunEuler2.z);
        
        Vector3 Euler = Boss2.rotation.eulerAngles;
        Boss2.rotation = Quaternion.Euler(0, Euler.y, 0);

        if (isFollowing && enemyCanFire)
        {

            GameObject p = Instantiate(eprojectile, efirePoint.position, efirePoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(efirePoint.forward * eprojVelocity);
            Destroy(p, eprojLifespan);

            GameObject p2 = Instantiate(eprojectile, efirePoint2.position, efirePoint2.rotation);
            p2.GetComponent<Rigidbody>().AddForce(efirePoint2.forward * eprojVelocity);
            Destroy(p2, eprojLifespan);

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
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);
            bossHealth--;
        }

        if (other.gameObject.tag == "explodingProjectile")
        {
            Destroy(other.gameObject);
            bossHealth--;
            GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
            Destroy(p, expTime);
        }
        if (other.gameObject.tag == "Explosion")
        {
            bossHealth--;
        }
    }

    IEnumerator ecooldownFire()
    {
        efireCooldown = false;
        yield return new WaitForSeconds(erof);
        enemyCanFire = true;
    }
}