using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    PlayerController player;

    [Header("Object Refrences")]
    public GameObject projectile;
    public Transform firepoint;
    public Camera firingDirection;

    [Header("Meta Attributes")]
    public bool canFire = true;
    public bool holdtoAttack = true;
    public bool reloading = false;
    public int weaponID;
    public string weaponName;

    [Header("Weapon Stats")]
    public float projLifespan;
    public float projVelocity;
    public float reloadCooldown;
    public float rof;
    public float firePoint;
    public int fireModes;
    public int currentFireMode;
    public int clip;
    public int clipSize;

    [Header("Weapon Stats")]
    public int ammo;
    public int maxAmmo;
    public int ammoRefill;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        firepoint = transform.GetChild(0);
        firingDirection = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void equip(PlayerController p)
    {
        player = p;

        player.currentWeapon = this;

        transform.SetPositionAndRotation(player.weaponSlot.position, player.weaponSlot.rotation);
        transform.SetParent(player.weaponSlot);

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent <Collider>().isTrigger = false;

        player = null;
    }
    public void unequip()
    {

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
    }

public void fire()
    {
    if (clip > 0 && canFire && !reloading)
        {
        clip--;

        GameObject p = Instantiate(projectile, firePoint.position, firePoint.rotation);
        p.GetComponent<Rigidbody>().AddForce(firingDirection.transform.forward * projVelocity);
        OnDestroy(p, projLifespan)
        
        }
    }

    
   
    IEnumerator burstDuration()
    {

    }

    IEnumerator cooldownFire()
    {
        yield return new WaitForSeconds(rof)

    }
    IEnumerator reloadingcCooldown()
    {

    }

    
}
