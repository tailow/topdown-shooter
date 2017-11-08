using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScript : MonoBehaviour {

    #region Variables

    Image spriteRenderer;

    #endregion

    void Start()
    {
        spriteRenderer = GameObject.Find("BlackScreen").GetComponent<Image>();
    }

    public void FadeIn(float fadeTime)
    {
        StartCoroutine("FadeInCoroutine", fadeTime);
    }

    public void FadeOut(float fadeTime)
    {
        StartCoroutine("FadeOutCoroutine", fadeTime);
    }

    IEnumerator FadeInCoroutine(float fadeTime)
    {
        float t = 0;

        while (true)
        {
            spriteRenderer.color = Color.Lerp(Color.black, new Color(0, 0, 0, 0), t);

            if (t < 1)
                t += Time.deltaTime / fadeTime;

            else
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOutCoroutine(float fadeTime)
    {
        float t = 0;

        while (true)
        {
            spriteRenderer.color = Color.Lerp(new Color(0, 0, 0, 0), Color.black, t);

            if (t < 1)
                t += Time.deltaTime / fadeTime;

            else
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
