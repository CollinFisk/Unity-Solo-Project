using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Slider healthBar;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();

        ammoText = GameObject.Find("ammoText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = player.health;

        if(player.currentWeapon)
        {
            ammoText.text = "Ammo: " + player.currentWeapon.ammo + "/" + player.currentWeapon.maxAmmo;
            clipText.text = "Clip: " + player.currentWeapon.clip + "/" + player.currentWeapon.clipSize;
        }
        else
        {
            weaponText.text = " ";
            ammoText.text = " ";
            clipText.text = " ";
        }
    }
}
