using System.Collections;
using System.Threading;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Splines;

public class Weapon : MonoBehaviour
{

    PlayerController player;

    [Header("Object Refrences")]
    public GameObject projectile;
    public GameObject explodingProjectile;
    public Transform firePoint;
    [Header("Meta Attributes")]
    public bool gun;
    public bool canFire = false;
    public bool holdToAttack = true;
    public bool reloading = false;
    public int weaponID;
    public string weaponName;

    [Header("Weapon Stats")]
    public float projLifespan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof;

    public int fireModes;
    public int currentFireMode;
    public int clip;
    public int clipSize;
    public int manaPrice;

    [Header("Ammo Stats")]
    public bool expWeapon;
    public bool shotgunWeapon;
    public bool regularWeapon;

    [Header("Weapon Position")]
    public float XOffset;
    public float YOffset;
    public float ZOffset;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firePoint = transform.GetChild(0);
    }
    public void equip(PlayerController p)
    {
        player = p;

        player.currentWeapon = this;

        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation);
        transform.SetParent(player.weaponSlot);
        canFire = true;

        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Collider>().isTrigger = true;
    }

    public void unequip()
    {
        player.currentWeapon = null;

        transform.SetParent(null);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Collider>().isTrigger = false;

        player = null;

    }

    public void reload()
    {
        if (clip >= clipSize)
            return;

        int reloadCount = clipSize - clip;

            clip += reloadCount;


        reloading = true;
        canFire = false;
        StartCoroutine("reloadingCooldown");
    }

    public void fire()
    {
        if (clip > 0 && canFire && !reloading && player.mana >= manaPrice)
        {

            if (regularWeapon)
            {
                GameObject p = Instantiate(projectile, firePoint.position, transform.rotation);
                p.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * projVelocity);
                Destroy(p, projLifespan);
            }
            if (expWeapon)
            {
                GameObject p = Instantiate(explodingProjectile, firePoint.position, transform.rotation);
                p.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * projVelocity);
                Destroy(p, projLifespan);
            }
            if (shotgunWeapon)
            {
                //repeat nine times
                for (int i = 0; i < 5; i++)
                {
                    Quaternion ShotgunShotRotation = Quaternion.identity;
                    ShotgunShotRotation.y = Random.Range(-1f, 1f);
                    ShotgunShotRotation.x = 0;
                    ShotgunShotRotation.z = 0;
                    ShotgunShotRotation.w = 0;
                    firePoint.rotation = ShotgunShotRotation;
                    GameObject p = Instantiate(projectile, firePoint.position, transform.rotation);
                    p.GetComponent<Rigidbody>().AddForce(firePoint.transform.forward * projVelocity);
                    Destroy(p, projLifespan);
                }
            }
            if (gun)
            {
                clip -= 1;
            }
            player.mana -= manaPrice;
            player.regenMana = false;
            player.canRegenMana = false;
            canFire = false;
            player.attackingAnim = true;
            player.movingAnim = false;
            StartCoroutine("cooldownFire");
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(rof);
        player.myAnim.SetBool("Attack", false);
        player.movingAnim = true;
        player.myAnim.SetBool("isMoving", true);


        if (clip > 0)
            canFire = true;
    }

    IEnumerator reloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);
        player.canRegenMana = true;
        reloading = false;
        canFire = true;
    }
}