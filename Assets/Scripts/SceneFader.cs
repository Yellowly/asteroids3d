using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;


//this whole script is copied from an old project
//i was meaning to use this for a "pause" menu but no time
public class SceneFader : MonoBehaviour
{
    public Image fadeImage;
    public AnimationCurve curve;
    public static SceneFader Instance;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());

    }
    public IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }
    }
    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    public void FadeTo(int scene)
    {
        StartCoroutine(FadeOut(scene));
    }
    public IEnumerator FadeOut(string scene = "null")
    {
        float t = 0f;
        while (t < 0f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }
        //Time.timeScale = 1;
        if (scene != "null")
        {
            SceneManager.LoadScene(scene);
        }
    }
    public IEnumerator FadeOut(int scene)
    {
        float t = 0f;
        while (t < 0f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            fadeImage.color = new Color(0f, 0f, 0f, t);
            yield return null;
        }
        //Time.timeScale = 1;
        if (scene != -1)
        {
            SceneManager.LoadScene(scene);
        }
        
    }

    public void FadeToNext()
    {
        FadeTo(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
