using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.PostProcessing;

public class ButtonScript : MonoBehaviour
{

    #region Variables

    GameObject fadeManager;

    GameObject menuCanvas;
    GameObject settingsCanvas;
    GameObject statisticsCanvas;

    GameObject canvasParent;

    public GameObject pauseMenuCanvas;
    public GameObject inGameSettingsCanvas;

    public PostProcessingProfile ppProfile;

    #endregion

    void Start()
    {
        fadeManager = GameObject.Find("FadeManager");

        InitializeCanvases();
    }

    void InitializeCanvases()
    {
        if (SceneManager.GetActiveScene().name == "scene_menu")
        {
            canvasParent = GameObject.Find("Canvases");

            menuCanvas = canvasParent.transform.GetChild(0).gameObject;
            settingsCanvas = canvasParent.transform.GetChild(1).gameObject;
            statisticsCanvas = canvasParent.transform.GetChild(2).gameObject;
        }
    }

    public void LoadGame()
    {
        StopAllCoroutines();

        StartCoroutine("LoadGameCoroutine");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadSettings()
    {
        pauseMenuCanvas.SetActive(false);

        inGameSettingsCanvas.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        pauseMenuCanvas.SetActive(true);

        inGameSettingsCanvas.SetActive(false);
    }

    public void LoadMenuSettings()
    {
        menuCanvas.SetActive(false);

        settingsCanvas.SetActive(true);

        ppProfile.depthOfField.enabled = false;
    }

    public void LoadMenuStatistics()
    {
        menuCanvas.SetActive(false);

        statisticsCanvas.SetActive(true);
    }

    public void BackToMenu()
    {
        menuCanvas.SetActive(true);

        statisticsCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("scene_menu");

        InitializeCanvases();

        Time.timeScale = 1;

        menuCanvas.SetActive(true);

        statisticsCanvas.SetActive(false);
        settingsCanvas.SetActive(false);

        ppProfile.depthOfField.enabled = false;
    }

    public void ContinueGame()
    {
        GameObject.FindGameObjectWithTag("PauseMenu").SetActive(false);

        ppProfile.depthOfField.enabled = false;

        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        GameObject.Find("DeathScreenCanvas").SetActive(false);

        SceneManager.LoadScene("scene_main");

        ppProfile.depthOfField.enabled = false;
    }

    IEnumerator LoadGameCoroutine()
    {
        float fadeTime = 0.5f;

        fadeManager.SendMessage("FadeOut", fadeTime);

        yield return new WaitForSeconds(fadeTime);

        Time.timeScale = 1;

        ppProfile.depthOfField.enabled = false;

        SceneManager.LoadScene("scene_main");
    }
}
