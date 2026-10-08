using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform door;

    public int DoorID;
    public PlayerController player;
    public GameManager gameManager;
    public GameObject requiredCollection;
    public GameObject Explosion;

    public Transform button;
    [Header("DoorReq")]

    public bool enemyReq;
    // If using a button door swith the script onto a button and child the door
    // Also make sure to give the door a projectile collisioins script
    public bool buttonReq;
    public bool collectionReq;
    public bool destroyDoorReq;
    public bool explosionDestroyDoorReq;
    public bool noReq;

    [Header("Req")]    
    public bool DoorOpen;
    public int enoughEnemiesKilled;
    public int doorHealth;
    public float expTime = 0.3f;
    public bool doorButtonPressed = false;


    void Start()
    {
        if (buttonReq)
        {
            button = transform;
            door = button.GetChild(0);
        }
        else
        {
            door = transform;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyReq)
        {
            if (gameManager.enemiesKilled >= enoughEnemiesKilled)
            {
                DoorOpen = true;
            }
        }
        if (collectionReq)
        {
            if (player.foundKey == true)
            {
                DoorOpen = true;
            }
        }
        if (destroyDoorReq || explosionDestroyDoorReq)
        {
            if (doorHealth <= 0)
            {
                DoorOpen = true;
            }
        }
        if (buttonReq)
        {
            if (doorButtonPressed)
            {
                DoorOpen = true;
            }
        }





        if (DoorOpen)
        {
            Destroy(door.gameObject);
        }
    }

    public void OnCollisionEnter(Collision other)
    {
        if (buttonReq)
        {
            if (other.gameObject.tag == "Projectile")
            {
                DoorOpen = true;
            }

            if (other.gameObject.tag == "explodingProjectile")
            {
                DoorOpen = true;

            }
            if (other.gameObject.tag == "Explosion")
            {
                DoorOpen = true;
            }
        }
        if (destroyDoorReq)
        {
            if (other.gameObject.tag == "Projectile")
            {
                Destroy(other.gameObject);
                doorHealth--;
            }

            if (other.gameObject.tag == "explodingProjectile")
            {
                Destroy(other.gameObject);
                doorHealth--;
                GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
                Destroy(p, expTime);
            }
            if (other.gameObject.tag == "Explosion")
            {
                doorHealth--;
            }
        }
        if (explosionDestroyDoorReq)
        {
            if (other.gameObject.tag == "Projectile")
            {
                Destroy(other.gameObject);
            }
            if (other.gameObject.tag == "explodingProjectile")
            {
                Destroy(other.gameObject);
                GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
                Destroy(p, expTime);
                DoorOpen = true;
            }
            if (other.gameObject.tag == "Explosion")
            {
                DoorOpen = true;
            }
        }
        if (enemyReq || noReq)
        {
            if (other.gameObject.tag == "Projectile")
            {
                Destroy(other.gameObject);
            }

            if (other.gameObject.tag == "explodingProjectile")
            {
                Destroy(other.gameObject);
                GameObject p = Instantiate(Explosion, other.collider.ClosestPoint(other.gameObject.transform.position), transform.rotation);
                Destroy(p, expTime);
            }
        }
    }
}
