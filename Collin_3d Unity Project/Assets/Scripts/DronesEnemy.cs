using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class DronesEnemy : MonoBehaviour
{
    public bool isFollowing = false;
    public bool enemyCanFire = true;
    public bool efireCooldown = true;
    public bool shot1;
    public bool shot2;
    public bool shot3;
    public bool shot4;


    public NavMeshAgent agent;
    public PlayerController player;

    public float detectionRadius = 20f;
    public float enemyHealth = 10;
    public float expTime = 0.3f;

    [Header("Object Refrences")]
    public GameObject Explosion;
    public GameObject eprojectile;

    public Transform efirePoint;
    public Transform efirePoint2;
    public Transform dualWeildRangedEnemy;
    public Transform eweaponSlot;
    public Transform eweaponSlot2;
    public Transform EnemyDrone;
    public Transform EnemyDrone2;

    public Transform efirePoint3;
    public Transform efirePoint4;
    public Transform eweaponSlot3;
    public Transform eweaponSlot4;
    public Transform EnemyDrone3;
    public Transform EnemyDrone4;

    [Header("Weapon Stats")]
    public float eprojLifespan;
    public float eprojVelocity;
    public float erof;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        eweaponSlot = dualWeildRangedEnemy.GetChild(0);
        eweaponSlot2 = dualWeildRangedEnemy.GetChild(1);
        EnemyDrone = eweaponSlot.GetChild(0);
        EnemyDrone2 = eweaponSlot2.GetChild(0);
        efirePoint = EnemyDrone.GetChild(0);
        efirePoint2 = EnemyDrone2.GetChild(0);

        eweaponSlot3 = dualWeildRangedEnemy.GetChild(2);
        eweaponSlot4 = dualWeildRangedEnemy.GetChild(3);
        EnemyDrone3 = eweaponSlot3.GetChild(0);
        EnemyDrone4 = eweaponSlot4.GetChild(0);
        efirePoint3 = EnemyDrone3.GetChild(0);
        efirePoint4 = EnemyDrone4.GetChild(0);

        dualWeildRangedEnemy = transform;

        EnemyDrone.SetPositionAndRotation(eweaponSlot.position, eweaponSlot.rotation);
        EnemyDrone.GetComponent<Rigidbody>().isKinematic = true;
        EnemyDrone.GetComponent<Collider>().isTrigger = true;

        EnemyDrone2.SetPositionAndRotation(eweaponSlot2.position, eweaponSlot2.rotation);
        EnemyDrone2.GetComponent<Rigidbody>().isKinematic = true;
        EnemyDrone2.GetComponent<Collider>().isTrigger = true;
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
            EnemyDrone.transform.LookAt(pos);
            EnemyDrone2.transform.LookAt(pos);
            EnemyDrone3.transform.LookAt(pos);
            EnemyDrone4.transform.LookAt(pos);
            dualWeildRangedEnemy.transform.LookAt(pos);

            Vector3 DroneEuler = EnemyDrone.rotation.eulerAngles;
            EnemyDrone.rotation = Quaternion.Euler(DroneEuler.x + 90, DroneEuler.y, DroneEuler.z);

            Vector3 DroneEuler2 = EnemyDrone2.rotation.eulerAngles;
            EnemyDrone2.rotation = Quaternion.Euler(DroneEuler2.x + 90, DroneEuler2.y, DroneEuler2.z);

            Vector3 DroneEuler3 = EnemyDrone3.rotation.eulerAngles;
            EnemyDrone3.rotation = Quaternion.Euler(DroneEuler3.x + 90, DroneEuler3.y, DroneEuler3.z);

            Vector3 DroneEuler4 = EnemyDrone4.rotation.eulerAngles;
            EnemyDrone4.rotation = Quaternion.Euler(DroneEuler4.x + 90, DroneEuler4.y, DroneEuler4.z);

            Vector3 Euler = dualWeildRangedEnemy.rotation.eulerAngles;
            dualWeildRangedEnemy.rotation = Quaternion.Euler(0, Euler.y, 0);
        }
        if (isFollowing && enemyCanFire)
        {
            if (shot1)
            {
                GameObject p = Instantiate(eprojectile, efirePoint.position, efirePoint.rotation);
                p.GetComponent<Rigidbody>().AddForce(efirePoint.forward * eprojVelocity);
                Destroy(p, eprojLifespan);
            }
            if (shot2)
            {
                GameObject p2 = Instantiate(eprojectile, efirePoint2.position, efirePoint2.rotation);
                p2.GetComponent<Rigidbody>().AddForce(efirePoint2.forward * eprojVelocity);
                Destroy(p2, eprojLifespan);
            }
            if (shot3)
            {
                GameObject p3 = Instantiate(eprojectile, efirePoint3.position, efirePoint3.rotation);
                p3.GetComponent<Rigidbody>().AddForce(efirePoint3.forward * eprojVelocity);
                Destroy(p3, eprojLifespan);
            }
            if (shot4)
            {
                GameObject p4 = Instantiate(eprojectile, efirePoint4.position, efirePoint4.rotation);
                p4.GetComponent<Rigidbody>().AddForce(efirePoint4.forward * eprojVelocity);
                Destroy(p4, eprojLifespan);
            }

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
            detectionRadius = 15f;
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

        if (shot1)
        {
            shot1 = false;
            shot2 = true;
        }

        else if(shot2)
        {
            shot2 = false;
            shot3 = true;
        }
        else if(shot3)
        {
            shot3 = false;
            shot4 = true;
        }
        else if(shot4)
        {
            shot4 = false;
            shot1 = true;
        }

            enemyCanFire = true;
    }
}