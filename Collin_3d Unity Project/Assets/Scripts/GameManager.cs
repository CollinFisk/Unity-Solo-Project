using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Image healthBar;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar = GameObject.Find("Healthbar").GetComponent<Image>();

        ammoText = GameObject.Find("ammoText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("WeaponName").GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float)player.health / (float)player.maxHealth;

        if(player.currentWeapon)
        {
            ammoText.text = "Ammo: " + player.currentWeapon.ammo + "/" + player.currentWeapon.maxAmmo;
            clipText.text = "Clip: " + player.currentWeapon.clip + "/" + player.currentWeapon.clipSize;
        }
        else
        {
            ammoText.text = " ";
            clipText.text = " ";
        }
    }
}
