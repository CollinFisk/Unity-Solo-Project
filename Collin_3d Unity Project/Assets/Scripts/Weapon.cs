using System.Collections;
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
    public bool canFire = true;
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

    [Header("Ammo Stats")]
    public int ammo;
    public int maxAmmo;
    public int ammoRefill;
    public bool expWeapon;
    public bool shotgunWeapon;
    public bool regularWeapon;

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

        if (ammo < reloadCount)
        {
            clip += ammo;
            ammo = 0;
        }
        else
        {
            clip += reloadCount;
            ammo -= reloadCount;
        }

        reloading = true;
        canFire = false;
        StartCoroutine("reloadingCooldown");
    }

    public void fire()
    {
        if (clip > 0 && canFire && !reloading)
        {
            clip--;

            if (!regularWeapon)
            {
                GameObject p = Instantiate(projectile, firePoint.position, transform.rotation);
                p.GetComponent<Rigidbody>().AddForce(player.transform.forward * projVelocity);
                Destroy(p, projLifespan);

                canFire = false;
                StartCoroutine("cooldownFire");
            }
            if (expWeapon)
            {
                GameObject p = Instantiate(explodingProjectile, firePoint.position, transform.rotation);
                p.GetComponent<Rigidbody>().AddForce(player.transform.forward * projVelocity);
                Destroy(p, projLifespan);

                canFire = false;
                StartCoroutine("cooldownFire");
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
                    GameObject p = Instantiate(projectile, firePoint.position, firePoint.rotation);


                    p.GetComponent<Rigidbody>().AddForce(player.transform.forward * projVelocity);
                    Destroy(p, projLifespan);
                }
                canFire = false;
                StartCoroutine("cooldownFire");
            }
        }
    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(rof);

        if (clip > 0)
            canFire = true;
    }

    IEnumerator reloadingCooldown()
    {
        yield return new WaitForSeconds(reloadCooldown);

        reloading = false;
        canFire = true;
    }
}