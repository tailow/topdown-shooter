using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PostProcessing;

public class GameManagerScript : MonoBehaviour
{

    #region Variables

    int zombieCount;
    int waveCount = 1;
    public static int zombiesKilled = 0;

    int amountOfEnemySpawns;
    int amountOfHealthpackSpawns;
    float timeBetweenEnemySpawns = 0.5f;
    float timeBetweenHealthpackSpawns = 30f;
    int bossWaveInterval = 5;

    public static bool controllerConnected = false;

    public static int zombieWaveCount;

    public GameObject healthpackPrefab;
    public GameObject bossPrefab;
    public GameObject enemyPrefab;
    public GameObject pauseMenu;

    GameObject fadeManager;

    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI zombiesLeftText;
    public TextMeshProUGUI healthText;

    public Text zombiesKilledText;
    public Text survivedWavesText;
    public Text timeSurvivedText;

    public PostProcessingProfile ppProfile;

    GameObject[] enemySpawnPoints;
    GameObject[] healthpackSpawnPoints;

    #endregion

    void Start()
    {
        fadeManager = GameObject.Find("FadeManager");

        fadeManager.SendMessage("FadeIn", 3.0f);

        // INITIALIZING TEXT
        #region Initialization
        currentWaveText.GetComponent<TextMeshPro>();
        healthText.GetComponent<TextMeshProUGUI>();
        zombiesLeftText.GetComponent<TextMeshProUGUI>();
        #endregion

        currentWaveText.SetText("Wave <#993399>" + waveCount);
        healthText.SetText("Health <#993399>" + PlayerScript.health);

        zombieWaveCount = waveCount * 4;
        zombiesLeftText.SetText("Zombies left <#993399>" + zombieWaveCount);

        // ADDING SPAWNS TO LIST
        #region Spawn listing

        // Enemies
        amountOfEnemySpawns = GameObject.Find("EnemySpawns").transform.childCount;
        enemySpawnPoints = new GameObject[amountOfEnemySpawns];

        for (int i = 0; i < amountOfEnemySpawns; i++)
        {
            enemySpawnPoints[i] = GameObject.Find("EnemySpawns").transform.GetChild(i).gameObject;
        }

        // Healthpacks
        amountOfHealthpackSpawns = GameObject.Find("HealthpackSpawns").transform.childCount;
        healthpackSpawnPoints = new GameObject[amountOfHealthpackSpawns];

        for (int i = 0; i < amountOfHealthpackSpawns; i++)
        {
            healthpackSpawnPoints[i] = GameObject.Find("HealthpackSpawns").transform.GetChild(i).gameObject;
        }
        #endregion

        StartCoroutine("StartNewWave");
        StartCoroutine("SpawnHealthpacks");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(!pauseMenu.activeInHierarchy);

            if (Time.timeScale == 0)
                Time.timeScale = 1;

            else if (Time.timeScale == 1)
                Time.timeScale = 0;

            ppProfile.depthOfField.enabled = !ppProfile.depthOfField.enabled;

        }

        // CHECK ZOMBIE COUNT
        zombieCount = GameObject.Find("Enemies").transform.childCount;

        healthText.SetText("Health <#993399>" + PlayerScript.health);
        zombiesLeftText.SetText("Zombies left <#993399>" + zombieWaveCount);
    }

    public void SetTexts()
    {
        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60f);
        int seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad - minutes * 60);

        string time = string.Format("{0:0}:{1:00}", minutes, seconds);

        //timeSurvivedText.text = "Time survived: " + time;
        //zombiesKilledText.text = "Zombies killed: " + zombiesKilled;
        //survivedWavesText.text = "Waves survived: " + (waveCount - 1);
    }

    #region Enemy Spawning

    IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(2f);

        if (waveCount % bossWaveInterval != 0)
        {
            zombieWaveCount = waveCount * 4;
        }

        else
        {
            zombieWaveCount = waveCount * 4 + 1;
        }

        currentWaveText.SetText("Wave <#993399>" + waveCount);

        yield return new WaitForSeconds(4f);

        // SPAWNING ENEMIES
        if (waveCount % bossWaveInterval != 0)
        {
            for (int i = 0; i < waveCount * 4; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(timeBetweenEnemySpawns);
            }
        }

        else
        {
            SpawnBoss();

            for (int i = 0; i < waveCount * 4; i++)
            {
                SpawnEnemy();

                yield return new WaitForSeconds(timeBetweenEnemySpawns);
            }
        }

        while (true)
        {
            if (zombieCount == 0)
            {
                waveCount++;

                break;
            }

            yield return null;
        }

        StartCoroutine("StartNewWave");

    }

    void SpawnEnemy()
    {
        int randomNumber = (int)Random.Range(0, amountOfEnemySpawns);

        var enemy = Instantiate(enemyPrefab, enemySpawnPoints[randomNumber].transform.position, Quaternion.Euler(0, 0, 0));

        enemy.transform.parent = GameObject.Find("Enemies").transform;
    }

    void SpawnBoss()
    {
        int randomNumber = (int)Random.Range(0, amountOfEnemySpawns);

        var boss = Instantiate(bossPrefab, enemySpawnPoints[randomNumber].transform.position, Quaternion.Euler(0, 0, 0));

        boss.transform.parent = GameObject.Find("Enemies").transform;
    }

    #endregion

    #region Healthpack Spawning

    IEnumerator SpawnHealthpacks()
    {
        // SPAWNING HEALTHPACKS

        while (true)
        {
            SpawnHealthpack();

            yield return new WaitForSeconds(timeBetweenHealthpackSpawns);
        }
    }

    void SpawnHealthpack()
    {
        int randomNumber = (int)Random.Range(0, amountOfHealthpackSpawns);

        var healthpack = Instantiate(healthpackPrefab, healthpackSpawnPoints[randomNumber].transform.position, Quaternion.Euler(0, 0, 0));

        healthpack.transform.parent = GameObject.Find("Healthpacks").transform;
    }

    #endregion
}
