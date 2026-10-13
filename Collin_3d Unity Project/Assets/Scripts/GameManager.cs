using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class GameManager : MonoBehaviour
{
    public PlayerController player;
    public Boss1 boss;

    public Interactions interactions;

    public Slider healthBar;
    public Slider bossHealthBar;


    public TextMeshProUGUI clipText;
    public TextMeshProUGUI weaponText;
    public TextMeshProUGUI pickupObjText;
    public TextMeshProUGUI pausedText;
    public TextMeshProUGUI gameOverText;


    public GameObject pauseMenu;
    public GameObject endDoor;

    public bool paused = false;
    public bool gameOver = false;
    public bool enemiesGone = false;

    public int enemyCount = 0;
    public int startEnemyCount = 0;
    public int enemiesKilled = 0;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        healthBar = GameObject.Find("healthBar").GetComponent<Slider>();
        bossHealthBar = GameObject.Find("bossHealthBar").GetComponent<Slider>();


        clipText = GameObject.Find("clipText").GetComponent<TextMeshProUGUI>();
        weaponText = GameObject.Find("weaponName").GetComponent<TextMeshProUGUI>();
        pickupObjText = GameObject.Find("PickupObjText").GetComponent<TextMeshProUGUI>();
        
        pausedText = GameObject.Find("pausedText").GetComponent<TextMeshProUGUI>();

        gameOverText = GameObject.Find("gameOverText").GetComponent<TextMeshProUGUI>();


        startEnemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("RangedEnemy").Length;
        interactions = GameObject.FindGameObjectWithTag("InteractSphere").GetComponent<Interactions>();

        enemyCount = startEnemyCount;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            healthBar.value = player.health;
            bossHealthBar.value = boss.bossHealth;

            if (player.currentWeapon)
            {
                clipText.text = "Clip: " + player.currentWeapon.clip + "/" + player.currentWeapon.clipSize;
                weaponText.text = player.currentWeapon.weaponName;
            }
            else
            {
                weaponText.text = " ";
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
            gameOverText.text = "GAME OVER";
        }
        else
            gameOverText.text = " ";

        if (player.health <= 0)
        {
            gameOver = true;
            Time.timeScale = 0;
        }

        if (boss.isFollowing)
        {
            bossHealthBar.gameObject.SetActive(true);
        }
        else
        {
            bossHealthBar.gameObject.SetActive(false);
        }
            enemiesKilled = startEnemyCount - enemyCount;
    }
        

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

    public void LoadLevel(int levelID)
    {
        if (levelID > SceneManager.sceneCountInBuildSettings)
            Debug.Log("Level ID is too high: " + levelID);
        else
            SceneManager.LoadScene(levelID);
    }

    public void LoadNextLevel()
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
}
