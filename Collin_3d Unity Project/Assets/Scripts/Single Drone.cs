using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SingleDrone : MonoBehaviour
{
    public bool isFollowing = false;
    public bool droneCanFire = true;
    public bool efireCooldown = true;


    public NavMeshAgent agent;
    public PlayerController player;
    public Vector2 efiringDirection;

    public float detectionRadius = 50f;
    public float droneHealth = 5f;
    public float expTime = 0.3f;

    [Header("Object Refrences")]
    public GameObject Explosion;
    public GameObject eprojectile;
    public Transform efirePoint;
    public Transform Rangeddrone;
    public Transform droneGun;

    [Header("Weapon Stats")]
    public float eprojLifespan;
    public float eprojVelocity;
    public float erof;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        droneGun = transform;
        efirePoint = droneGun.GetChild(0);
        efiringDirection = Rangeddrone.forward;

        droneGun.GetComponent<Rigidbody>().isKinematic = true;
        droneGun.GetComponent<Collider>().isTrigger = true;
        if (transform.parent != null)
        {
            detectionRadius = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (droneHealth <= 0)
        {
            isFollowing = false;
            Destroy(gameObject);
        }

        GetComponent<SphereCollider>().radius = detectionRadius;
        if (isFollowing)
        {
            agent.destination = player.transform.position;


            var pos = player.transform.position;

            transform.LookAt(pos);

            Vector3 gunEuler = transform.rotation.eulerAngles;
            droneGun.rotation = Quaternion.Euler(gunEuler.x + 90, gunEuler.y, gunEuler.z);
        }


        if (isFollowing && droneCanFire)
        {

            GameObject p = Instantiate(eprojectile, efirePoint.position, efirePoint.rotation);
            p.GetComponent<Rigidbody>().AddForce(efirePoint.forward * eprojVelocity);
            Destroy(p, eprojLifespan);
            efireCooldown = true;
            droneCanFire = false;
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
            if (!droneCanFire && efireCooldown)
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
            droneHealth--;
        }

        if (other.gameObject.tag == "explodingProjectile")
        {
            Destroy(other.gameObject);
            droneHealth--;
            GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
            Destroy(p, expTime);
        }
        if (other.gameObject.tag == "Explosion")
        {
            droneHealth--;
        }
    }

    IEnumerator ecooldownFire()
    {
        efireCooldown = false;
        yield return new WaitForSeconds(erof);
        droneCanFire = true;
    }
}