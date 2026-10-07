using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public PlayerController player;

    public Interactions interactions;

    public Slider healthBar;

    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI pickupObjText;
    public TextMeshProUGUI pausedText;
    public TextMeshProUGUI gameOverText;


    public GameObject pauseMenu;
    public GameObject door;

    public bool paused = false;
    public bool gameOver = false;
    public bool enemiesGone = false;

    public int enemyCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();

        ammoText = GameObject.Find("ammoText").GetComponent<TextMeshProUGUI>();
        clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("weaponName").GetComponent<TextMeshProUGUI>();
        pickupObjText = GameObject.Find("PickupObjText").GetComponent<TextMeshProUGUI>();
        
        pausedText = GameObject.Find("pausedText").GetComponent<TextMeshProUGUI>();

        gameOverText = GameObject.Find("gameOverText").GetComponent<TextMeshProUGUI>();


        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("RangedEnemy").Length;
        interactions = GameObject.FindGameObjectWithTag("InteractSphere").GetComponent<Interactions>();
        door = GameObject.FindWithTag("EndDoor");


    }

    // Update is called once per frame
    void Update()
    {
        if (!paused || !gameOver)
        {
            healthBar.value = player.health;

            if (player.currentWeapon)
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

            if (player.pickupObj != null)
            {
                pickupObjText.text = player.pickupObj.name;
            }
            else
                pickupObjText.text = " ";
        }
        if (paused)
        {
            pausedText.text = "Paused";
        }
        else
            pausedText.text = " ";

        if (gameOver)
        {

        }

        if (enemyCount <= 0)
        {
            Destroy(door.gameObject);
        }
        if (player.health <= 0)
        {
            gameOver = true;
            Time.timeScale = 0;
        }
    }

    //No pause Object is set up yet
    public void Pause()
    {
        if (!gameOver)
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
