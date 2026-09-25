using System.Collections;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemies : MonoBehaviour
{
    public bool isFollowing = false;

    public NavMeshAgent agent;
    public PlayerController player;
    public Vector2 efiringDirection;
   
    public float detectionRadius = 20f;

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

    public bool enemyCanFire = true;
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

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }


    // Update is called once per frame
    void Update()
    {
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
            detectionRadius = 25f;
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
    public void fire()
    {
        if (isFollowing && enemyCanFire)
        {

            GameObject p = Instantiate(eprojectile, efirePoint.position, efirePoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(efiringDirection * eprojVelocity);
            Destroy(p, eprojLifespan);
            enemyCanFire = false;
            StartCoroutine("ecooldownFire");
        }
    }

    IEnumerator ecooldownFire()
    {
        yield return new WaitForSeconds(erof);
        enemyCanFire = true;
    }
}