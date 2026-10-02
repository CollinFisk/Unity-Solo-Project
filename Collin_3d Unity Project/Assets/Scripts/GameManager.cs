using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Slider healthBar;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;

    public GameObject pauseMenu;

    public bool paused = false;
    public bool enemiesGone = false;

    public int enemyCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

      //  pauseMenu = GameObject.FindGameObjectWithTag("Pause");
      //  pauseMenu.SetActive(false);

        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();

        ammoText = GameObject.Find("ammoText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("weaponName").GetComponent<TextMeshProUGUI>();

        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("RangedEnemy").Length;

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.value = player.health;

        if(player.currentWeapon)
        {
            ammoText.text = "Ammo: " + player.currentWeapon.ammo + "/" + player.currentWeapon.maxAmmo;
            clipText.text = "Clip: " + player.currentWeapon.clip + "/" + player.currentWeapon.clipSize;
            weaponText.text = player.currentWeapon.weaponName;
        }
        else
        {
            weaponText.text = " ";
            ammoText.text = " ";
            clipText.text = " ";
        }
    }
    public void Pause()
    {
        paused = !paused;

        pauseMenu.SetActive(paused);

        if (paused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    /*
    public void LoadLevel(int levelID)
    {
        if (levelID >= SceneManager.sceneCountInBuildSettings)
            Debug.Log("Level ID is too high: " + levelID);
        else
            SceneManager.LoadScene(levelID);
    }

    public void LoadNextNevel()
    {
        LoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void MainMenu()
    {
        LoadLevel(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
    */
}
