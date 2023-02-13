using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManage : MonoBehaviour
{
    public static SceneManage instance;
    public GameObject fadeOutCanvas;
    public GameObject fadeInCanvas;
    [Range(0, 1)]public float waitTime = 0.49f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SceneFadeIn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ReloadScene();
    }

    public void ReloadScene()
    {
        StartCoroutine(SceneFadeOutLoad(0));
    }

    public void NextScene()
    {
        if(SceneManager.GetActiveScene().buildIndex != 6)
            StartCoroutine(SceneFadeOutLoad(1));
        else
            StartCoroutine(SceneFadeOutLoad(3));
    }

    public void LastScene()
    {
        StartCoroutine(SceneFadeOutLoad(2));
    }

    IEnumerator SceneFadeOutLoad(int whichScene)
    {
        fadeOutCanvas.SetActive(true);
        yield return new WaitForSecondsRealtime(waitTime);
        switch (whichScene) // 0 reload this level; 1 load next level; 2 load previous level 
        {
            case 0:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            case 1:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case 2:
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
                break;
            case 3:
                SceneManager.LoadScene(0);
                break;
            default:
                Debug.LogError("Wrong scene number!");
                break;
        }
    }

    IEnumerator SceneFadeIn()
    {
        if (fadeInCanvas)
        {
            fadeInCanvas.SetActive(true);
            yield return new WaitForSecondsRealtime(waitTime);
            fadeInCanvas.SetActive(false);
        }
    }
}
