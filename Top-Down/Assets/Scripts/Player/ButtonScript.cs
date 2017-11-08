using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour {

    #region Variables

    GameObject fadeManager;

    #endregion

    void Start()
    {
        fadeManager = GameObject.Find("FadeManager");
    }

    public void LoadGame()
    {
        PlayerScript.health = 3;

        StopAllCoroutines();
        StartCoroutine("LoadGameCoroutine");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadSettings()
    {
        PlayerScript.health = 3;

        SceneManager.LoadScene("scene_settings");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("scene_menu");
        PlayerScript.health = 3;

        Time.timeScale = 1;
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

        PlayerScript.health = 3;

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
