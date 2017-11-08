using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManagerScript : MonoBehaviour {

    #region Variables

    int zombieCount;
    int waveCount = 1;
    public static int zombiesKilled = 0;

    int amountOfSpawns;
    float timeBetweenSpawns = 0.5f;

    public static bool controllerConnected = false;

    public static int zombieWaveCount;

    public GameObject enemyPrefab;
    public GameObject pauseMenu;

    GameObject fadeManager;

    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI zombiesLeftText;
    public TextMeshProUGUI healthText;

    public Text zombiesKilledText;
    public Text survivedWavesText;
    public Text timeSurvivedText;

    GameObject[] spawnPoints;

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

        currentWaveText.SetText("Wave: <#FF8000>" + waveCount);
        healthText.SetText("Health: <#FF8000>" + PlayerScript.health);

        zombieWaveCount = waveCount * 5;
        zombiesLeftText.SetText("Zombies left: <#FF8000>" + zombieWaveCount);     

        // ADDING SPAWNS TO LIST
        #region Spawn listing
        amountOfSpawns = GameObject.Find("SpawnPoints").transform.childCount;
        spawnPoints = new GameObject[amountOfSpawns];

        for (int i = 0; i < amountOfSpawns; i++)
        {
            spawnPoints[i] = GameObject.Find("SpawnPoints").transform.GetChild(i).gameObject;
        }
        #endregion

        StartCoroutine("StartNewWave");
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
        }      

        // CHECK ZOMBIE COUNT
        zombieCount = GameObject.Find("Enemies").transform.childCount;

        healthText.SetText("Health: <#FF8000>" + PlayerScript.health);
        zombiesLeftText.SetText("Zombies left: <#FF8000>" + zombieWaveCount);
    }

    public void SetTexts()
    {
        int minutes = Mathf.FloorToInt(Time.timeSinceLevelLoad / 60f);
        int seconds = Mathf.FloorToInt(Time.timeSinceLevelLoad - minutes * 60);

        string time = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeSurvivedText.text = "Time survived: " + time;
        zombiesKilledText.text = "Zombies killed: " + zombiesKilled;
        survivedWavesText.text = "Waves survived: " + (waveCount - 1);
    }

    #region Enemy Spawning

    IEnumerator StartNewWave()
    {
        yield return new WaitForSeconds(2f);

        zombieWaveCount = waveCount * 5;
        currentWaveText.SetText("Wave: <#FF8000>" + waveCount);

        yield return new WaitForSeconds(4f);      

        // SPAWNING ENEMIES
        for (int i = 0; i < waveCount * 5; i++)
        {
            SpawnEnemy();

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        while (true)
        {
            if(zombieCount == 0)
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
        int randomNumber = (int)Random.Range(0, amountOfSpawns);

        var enemy = Instantiate(enemyPrefab, spawnPoints[randomNumber].transform.position, Quaternion.Euler(0, 0, 0));

        enemy.transform.parent = GameObject.Find("Enemies").transform;

    }

    #endregion
}
