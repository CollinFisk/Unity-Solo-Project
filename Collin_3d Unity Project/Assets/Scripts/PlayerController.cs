using JetBrains.Annotations;
using System.Collections;
using System.Data.SqlTypes;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class PlayerController : MonoBehaviour
{
    public Animator myAnim;

    public int health = 5;
    public int maxHealth = 5;

    public float sprintSpeed = 30.0f;
    public float speed = 15.0f;
    public float jumpHeight = 5.0f;
    public float jumpDetectDistance = 1.1f;
    public float interactDistance = 5f;
    public float fusionDmgInterval = 1f;
    public float enemyAttackRate = 1f;
    public float dashStrength;
    public float dashCooldownSpeed = 2f;
    public float dashDuration = 0.2f;

    public float interactCooldown = 2f;

    public Transform InteractSphere;
    public Interactions canInteract;
    public Interactions interactions;
    public GameManager gameManager;
    public bool attacking = false;
    public bool sprinting = false;
    public bool dashing = false;
    public bool fusionDmg = false;
    public bool enemyDamage = false;
    public bool foundKey = false;
    public bool canDash = true;
    public bool attackingAnim;
    public bool movingAnim;

    public float mana;
    public float maxMana;
    public float manaRefill;
    public float manaCooldown;
    public bool manaLock;
    public bool regenMana;
    public bool canRegenMana;

    RaycastHit mouseCheck;
    Vector2 moveInput = Vector2.zero;
    Vector3 worldPos;

    public Weapon currentWeapon;

    Camera playerCam;
    public Transform weaponSlot;
    PlayerInput input;
    Rigidbody rb;
    Transform firingSurface;
    public GameObject pickupObj;
    public GameObject EnemyAttackHitbox;
    public GameObject fireTo;
    public GameObject Explosion;

    public Transform playerMovement;
    public Collider coll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        playerCam = Camera.main;
        weaponSlot = transform.GetChild(0);

        InteractSphere = GameObject.Find("InteractSphere").transform;
        interactions = InteractSphere.GetComponent<Interactions>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerMovement = GameObject.Find("playerMovement").GetComponent<Transform>();

        firingSurface = GameObject.FindGameObjectWithTag("FS").transform;
        fireTo = GameObject.Find("fireTo");
        coll = GameObject.Find("FireSurface").GetComponent<Collider>();

        if (transform.GetComponent<Animator>() != null)
        {
            myAnim = GetComponent<Animator>();
        }
        movingAnim = true;
    }

    private void FixedUpdate()
    {
        Quaternion playerMovementRotation = Quaternion.identity;
        playerMovementRotation.y = playerCam.transform.rotation.y;
        playerMovementRotation.x = 0;
        playerMovementRotation.z = 0;
        playerMovement.rotation = playerMovementRotation;
        if (!dashing)
        {
            Vector3 horizontalMovement = (moveInput.x * speed * playerMovement.right) + (moveInput.y * speed * playerMovement.forward);

            rb.linearVelocity = new Vector3(horizontalMovement.x, rb.linearVelocity.y, horizontalMovement.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        coll.Raycast(playerCam.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono), out mouseCheck, 10000000);

        firingSurface.transform.position = new Vector3 (transform.position.x, transform.position.y - 1.3f, transform.position.z);

        worldPos = mouseCheck.point;

        fireTo.transform.position = worldPos;

        InteractSphere.position = transform.position;
        InteractSphere.rotation = transform.rotation;

        if (gameManager.gameOver != true && gameManager.paused != true)
        {
            if (mouseCheck.transform != null)
            {
                transform.LookAt(worldPos);
                Vector3 euler = transform.rotation.eulerAngles;
                transform.rotation = Quaternion.Euler(0, euler.y, 0);

                if (currentWeapon)
                {
                    weaponSlot.transform.LookAt(worldPos);

                    Vector3 gunEuler = weaponSlot.transform.rotation.eulerAngles;
                    weaponSlot.transform.rotation = Quaternion.Euler(gunEuler.x + 80, gunEuler.y, gunEuler.z);

                    Vector3 weaponPos = transform.position;
                    weaponPos.x = transform.position.x + currentWeapon.XOffset;
                    weaponPos.y = transform.position.y + currentWeapon.YOffset;
                    weaponPos.z = transform.position.z + currentWeapon.ZOffset;
                    weaponSlot.transform.position = weaponPos;

                    if (currentWeapon.holdToAttack && attacking)
                    {
                        currentWeapon.fire();
                    }
                }
            }

            if (mana <= 0)
            {
                canRegenMana = false;
                mana = 0;
            }

            else if (!canRegenMana && !manaLock)
            {
                StartCoroutine("manaReset");
            }
            else if (canRegenMana && !regenMana)
            {
                regenMana = true;
            }
            else if (regenMana)
            {
                mana += manaRefill * Time.fixedDeltaTime;

                if (mana >= maxMana)
                {
                    mana = maxMana;
                }
            }
        }

        if (myAnim != null)
        {
            if (movingAnim)
            {
                if (rb.linearVelocity != Vector3.zero)
                {
                    myAnim.SetBool("isMoving", true);
                }
                else
                    myAnim.SetBool("isMoving", false);
            }
            else
            {
                myAnim.SetBool("isMoving", false);
            }
            if (attackingAnim)
            {
                myAnim.SetBool("isMoving", false);
                myAnim.SetBool("Attack", true);
                attackingAnim = false;
            }
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.ReadValueAsButton())
        {
            if (pickupObj)
            {
                if (interactions.canInteract)
                {
                    if (pickupObj.tag == "Weapon")
                    {
                        if (currentWeapon)
                        {
                            DropWeapon();
                            pickupObj.GetComponent<Weapon>().equip(this);
                            pickupObj = null;
                        }
                        else
                        {
                            pickupObj.GetComponent<Weapon>().equip(this);
                            pickupObj = null;
                        }
                    }
                    else if (pickupObj.tag == "collectionReq")
                    {
                        Destroy(pickupObj);
                        foundKey = true;
                    }
                    else if (pickupObj.tag == "Door")
                    {
                        Destroy(pickupObj);
                        interactions.canInteract = false;
                    }
                    else if (pickupObj.tag == "Button")
                    {
                        pickupObj.GetComponent<Door>().doorButtonPressed = true;
                    }
                    else if (pickupObj.tag == "Health")
                    {
                        Destroy(pickupObj);
                        health = maxHealth;
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
        if (currentWeapon && gameManager.gameOver == false && gameManager.paused == false)
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

    public void Dash(InputAction.CallbackContext context)
    {
        if (!dashing && canDash)
        {
            StartCoroutine("dashCooldown");
        }
    }


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
        if (!dashing)
        {

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

            if (collision.gameObject.tag == "FusionHazard")
            {
                health--;
            }

            if (collision.gameObject.tag == "eprojectile")
            {
                health--;
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.tag == "Projectile")
            {
                health--;
                Destroy(collision.gameObject);
            }

            if (collision.gameObject.tag == "explodingProjectile")
            {
                Destroy(collision.gameObject);
                health--;
                GameObject p = Instantiate(Explosion, collision.collider.ClosestPoint(collision.gameObject.transform.position), transform.rotation);
                Destroy(p, 0.3f);
            }

            if (collision.gameObject.tag == "Health" && health < maxHealth)
            {
                health = maxHealth;

                Destroy(collision.gameObject);
            }
            if (collision.gameObject == playerMovement)
                collision=null;
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
        if (other.gameObject.tag == "EndDoor")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().LoadNextLevel();
        }

        if (other.gameObject.tag == "Explosion")
        {
            health = health - 3;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttackHitbox")
        {
            if (!enemyDamage)
            {
                StartCoroutine("enemyDmgCooldown");
                if (other.GetComponentInParent<Enemy>() == true)
                    other.GetComponentInParent<Enemy>().attackingAnim = true;
                else if (other.GetComponentInParent<Boss1>() == true)
                    other.GetComponentInParent<Boss1>().attackingAnim = true;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "EnemyAttackHitbox")
        {
            if (enemyDamage)
            {
                StopCoroutine("enemyDmgCooldown");
                enemyDamage = false;
            }
            if (other.GetComponentInParent<Enemy>() == true)
            {
                other.GetComponentInParent<Enemy>().attackingAnim = false;
                other.GetComponentInParent<Animator>().SetBool("Attack", false);
            }
            if (other.GetComponentInParent<Boss1>() == true)
                other.GetComponentInParent<Boss1>().attackingAnim = false;
                other.GetComponentInParent<Animator>().SetBool("Attack", false);
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

    IEnumerator dashCooldown()
    {
        canDash = false;
        dashing = true;

        Vector3 currentVelocity = rb.linearVelocity;
        rb.linearVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);

        Vector3 dashDirection = transform.forward;

        rb.AddForce(dashDirection * dashStrength, ForceMode.Impulse);

        yield return new WaitForSeconds(dashDuration);
        dashing = false;

        yield return new WaitForSeconds(dashCooldownSpeed);
        canDash = true;
    }

    IEnumerator manaReset()
    {
        manaLock = true;
        regenMana = false;
        yield return new WaitForSeconds(manaCooldown);

        canRegenMana = true;
        regenMana = true;
        manaLock = false;
    }
}