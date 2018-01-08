using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{

    #region Variables

    GameObject fadeManager;

    public GameObject menuCanvas;
    public GameObject settingsCanvas;
    public GameObject statisticsCanvas;

    #endregion

    void Start()
    {
        fadeManager = GameObject.Find("FadeManager");
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

    public void LoadMenuSettings()
    {
        menuCanvas.SetActive(false);

        settingsCanvas.SetActive(true);
    }

    public void LoadMenuStatistics()
    {
        menuCanvas.SetActive(false);

        statisticsCanvas.SetActive(true);
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("scene_menu");

        Time.timeScale = 1;

        menuCanvas.SetActive(true);

        statisticsCanvas.SetActive(false);
        settingsCanvas.SetActive(false);
    }

    public void ContinueGame()
    {
        GameObject.FindGameObjectWithTag("PauseMenu").SetActive(false);
        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        GameObject.Find("DeathScreen").SetActive(false);

        SceneManager.LoadScene("scene_main");
    }

    IEnumerator LoadGameCoroutine()
    {
        float fadeTime = 0.5f;

        fadeManager.SendMessage("FadeOut", fadeTime);

        yield return new WaitForSeconds(fadeTime);

        Time.timeScale = 1;
        SceneManager.LoadScene("scene_main");
    }
}
