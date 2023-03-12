using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private readonly string GAME_SCENE_NAME = "SampleInGameScene";

    public GameObject loadScreenGameObject;
    public CanvasGroup loadScreenCanvas;
    public Animator loadingAnimator;

    public GameObject pauseScreen;

    #region MonoBehavior
    protected override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoadEnd;

        ArchieveManager.instance.gameObject.SetActive(false);
        InGameManager.instance.gameObject.SetActive(false);
        WaveManager.instance.gameObject.SetActive(false);
        UpgradeManager.instance.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseScreen.activeSelf)
            {
                ClosePauseScreen();
            } else
            {
                OpenPauseScreen();
            }
        }
    }
    #endregion

    #region Loading
    private IEnumerator Loading(string sceneName)
    {
        yield return StartCoroutine(Fade(true, 0.3f));

        OnSceneLoadStart(sceneName);

        yield return new WaitForSecondsRealtime(0.5f);

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        while (!op.isDone)
        {
            yield return null;
        }
    }

    private IEnumerator Fade(bool isFadeIn, float duration)
    {

        float timer = 0f, start, end;

        if (isFadeIn)
        {
            start = 0f;
            end = 1f;
            loadScreenGameObject.SetActive(true);
        } else
        {
            start = 1f;
            end = 0f;
        }

        loadScreenCanvas.alpha = start;

        while (timer < duration)
        {
            yield return null;

            timer += Time.unscaledDeltaTime;
            loadScreenCanvas.alpha = Mathf.Lerp(start, end, timer / duration);
        }

        if (!isFadeIn)
        {
            loadScreenGameObject.SetActive(false);
        }

        if (isFadeIn)
        {
            OnFadeFull();
        } else
        {
            OnFadeEnd();
        }

        yield return null;
    }

    private void OnSceneLoadStart(string sceneName)
    {
        
    }

    private void OnSceneLoadEnd(Scene scene, LoadSceneMode mode)
    {
        loadingAnimator.SetTrigger("LoadEnd");

        StartCoroutine(Fade(false, 0.3f));

        if (scene.name == GAME_SCENE_NAME)
        {
            ArchieveManager.instance.gameObject.SetActive(true);
            InGameManager.instance.gameObject.SetActive(true);
            WaveManager.instance.gameObject.SetActive(true);
            UpgradeManager.instance.gameObject.SetActive(true);

            ArchieveManager.instance.InitOnStartGame();
            InGameManager.instance.InitOnStartGame();
            WaveManager.instance.InitOnStartGame();
            UpgradeManager.instance.InitOnStartGame();
        } else
        {
            ArchieveManager.instance.gameObject.SetActive(false);
            InGameManager.instance.gameObject.SetActive(false);
            WaveManager.instance.gameObject.SetActive(false);
            UpgradeManager.instance.gameObject.SetActive(false);
        }
    }

    private void OnFadeFull()
    {
        loadingAnimator.SetTrigger("LoadStart");
    }

    private void OnFadeEnd()
    {

    }
    #endregion

    #region GoToGame
    public void StartGame()
    {
        StartCoroutine(Loading(GAME_SCENE_NAME));
    }

    public void BackToTitle()
    {
        StartCoroutine(Loading("TitleScene"));
    }
    #endregion

    #region PauseScreen
    public void OpenPauseScreen()
    {
        if (SceneManager.GetActiveScene().name != GAME_SCENE_NAME)
        {
            return;
        }

        if (pauseScreen.activeSelf)
        {
            return;
        }

        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ClosePauseScreen()
    {
        if (!pauseScreen.activeSelf)
        {
            return;
        }

        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }
    #endregion
}