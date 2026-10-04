using JetBrains.Annotations;
using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public int health = 5;
    public int maxHealth = 5;

    public float sprintSpeed = 30.0f;
    public float speed = 15.0f;
    public float jumpHeight = 5.0f;
    public float jumpDetectDistance = 1.1f;
    public float interactDistance = 5f;
    public float fusionDmgInterval = 1;
    public float enemyAttackRate = 1;

    public float interactCooldown = 2;

    public Transform InteractSphere;

    public Interactions canInteract;
    public bool attacking = false;
    public bool sprinting = false;
    public bool crouching = false;
    public bool fusionDmg = false;
    public bool enemyDamage = false;

    Ray jumpRay;
    RaycastHit interactHit;
    RaycastHit mouseCheck;
    Vector2 moveInput = Vector2.zero;
    Vector3 worldPos;
    Vector3 movePos;

    public Weapon currentWeapon;

    Camera playerCam;
    public Transform weaponSlot;
    PlayerInput input;
    Rigidbody rb;
    Transform firingSurface;
    public GameObject pickupObj;
    public GameObject EnemyAttackHitbox;
    public GameObject fireTo;
    public Collider coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();
        jumpRay = new Ray();
        playerCam = Camera.main;
        weaponSlot = transform.GetChild(0);

        InteractSphere = GameObject.Find("InteractSphere").transform;
        firingSurface = GameObject.FindGameObjectWithTag("FS").transform;
        fireTo = GameObject.Find("fireTo");
        coll = GameObject.Find("FireSurface").GetComponent<Collider>();


    }

    private void FixedUpdate()
    {
        Quaternion playerRotation = Quaternion.identity;
        playerRotation.y = playerCam.transform.rotation.y;
        playerRotation.x = 0;
        playerRotation.z = 0;

        transform.rotation = playerRotation;
        
        Vector3 tempMove = rb.linearVelocity;

        tempMove.x = moveInput.x * speed;
        tempMove.z = moveInput.y * speed;

        rb.linearVelocity = (tempMove.x * transform.right) +
                                (tempMove.y * transform.up) +
                                (tempMove.z * transform.forward);
    }

    // Update is called once per frame
    void Update()
    {
        coll.Raycast(playerCam.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono), out mouseCheck, 10000000);

        if(mouseCheck.transform != null)
            worldPos = mouseCheck.point;

        firingSurface.position = transform.position;

        transform.LookAt(worldPos);

        Vector3 euler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, euler.y, 0);

        InteractSphere.position = transform.position;
        InteractSphere.rotation = transform.rotation;

        fireTo.transform.position = worldPos;
        //fireTo.transform.position = new Vector3(fireTo.transform.position.x, transform.position.y, fireTo.transform.position.z);

        if (health <= 0)
        {

        }

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && attacking)
                currentWeapon.fire();
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    /*
    public void Jump()
    {
        if (Physics.Raycast(jumpRay, jumpDetectDistance))
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
    }
    */
    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pickupObj)
            {
                if (pickupObj.tag == "Weapon")
                {
                    if (canInteract)
                    {
                        if (currentWeapon)
                        {
                            DropWeapon();
                            pickupObj.GetComponent<Weapon>().equip(this);
                        }
                        else
                        {
                            pickupObj.GetComponent<Weapon>().equip(this);
                        }
                    }
                }
            }
        }
    }

    public void Reload(InputAction.CallbackContext context)
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    attacking = true;
                else
                    attacking = false;
            }

            else if (context.ReadValueAsButton())
                currentWeapon.fire();
        }
    }
    public void Sprint(InputAction.CallbackContext context)
    {
        if (sprinting == false)
        {
            speed = (sprintSpeed);
            sprinting = true;
        }
        else
        {
            speed = 10;
            sprinting = false;
        }
    }
    /*
    public void Crouch(InputAction.CallbackContext context)
    {
        if (!crouching)
        {
            transform <ScaleMode>(1, 0.5, 1);
            crouching = true;
        }
        else
        {
            crouching = false; 
        }
    }
    */

    public void Unequip(InputAction.CallbackContext context)
    {
        if (currentWeapon)
            if (canInteract)
                DropWeapon();
    }
    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.GetComponent<Weapon>().unequip();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ammo" && currentWeapon && currentWeapon.ammo < currentWeapon.maxAmmo)
        {
            int refillAmt = currentWeapon.ammo + currentWeapon.ammoRefill;

            if (refillAmt >= currentWeapon.maxAmmo)
            {
                currentWeapon.ammo = currentWeapon.maxAmmo;
            }
            else
                currentWeapon.ammo += currentWeapon.ammoRefill;

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Hazard")
        {
            health--;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            health--;
        }

        if (collision.gameObject.tag == "RangedEnemy")
        {
            health--;
        }

        if (collision.gameObject.tag == "EnemyAttack")
        {
            health--;
        }

        if (collision.gameObject.tag == "eprojectile")
        {
            health--;
        }

        if (collision.gameObject.tag == "FusionHazard")
        {
            health--;
        }

        if (collision.gameObject.tag == "Health" && health < maxHealth)
        {
            health++;

            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "FusionHazard")
        {
            if (!fusionDmg)
            {
                StartCoroutine("fusionDmgCooldown");
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "LevelEnd")
        {
            SceneManager.LoadScene(0);
        }

        if (other.gameObject.tag == "Weapon")
            pickupObj = other.gameObject;
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttackHitbox")

        {
            if (!enemyDamage)
            {
                StartCoroutine("enemyDmgCooldown");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Weapon")
            pickupObj = null;

        if (other.gameObject.tag == "EnemyAttackHitbox")
        {
            if (enemyDamage)
            {
                StopCoroutine("EnemyAttackHitbox");
                enemyDamage = false;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "FusionHazard")
        {
            if (fusionDmg)
            {
                StopCoroutine("fusionDmgCooldown");
                fusionDmg = false;
            }
        }
    }

    IEnumerator fusionDmgCooldown()
    {
        fusionDmg = true;

        yield return new WaitForSeconds(fusionDmgInterval);

        health--;
        fusionDmg = false;
    }

    IEnumerator enemyDmgCooldown()
    {
        enemyDamage = true;

        yield return new WaitForSeconds(enemyAttackRate);

        health--;
        enemyDamage = false;
    }
}