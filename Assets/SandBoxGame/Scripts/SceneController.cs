using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    public Button gameStartButton;
    public Button gameSettingButton;
    public Button gameQuit;
    public Button gameGoBackButton;

    public GameObject SettingsUI;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        StartCoroutine(MenuStart());
        SettingsUI.SetActive(false);
    }

    public void GameStart() //게임시작 : GameScene 호출
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex; //현재 씬의 빌드 인덱스 가져오기

        //로딩창, 페이드인 페이드아웃 필요

        SceneManager.LoadScene(currentSceneIndex + 1); //현재 씬의 다음 인덱스 씬으로 이동
    }

    public void GameSetting()
    {
        SettingsUI.SetActive(true);
    }

    public void GoBackButton()
    {
        SettingsUI.SetActive(false);
        Debug.Log("뒤로 가기");
    }

    public void GameExit()
    {
        Application.Quit();
    }

    IEnumerator MenuStart()
    {
        GameObject startButtonObj = GameObject.Find("StartButton");
        GameObject settingButtonObj = GameObject.Find("SettingButton");
        GameObject quitButtonObj = GameObject.Find("ExitButton");
        GameObject goBackButtonObj = GameObject.Find("GoBackButton");

        if (startButtonObj != null)
        {
            gameStartButton = startButtonObj.GetComponent<Button>();
            gameStartButton.onClick.AddListener(GameStart);
        }

        if (settingButtonObj != null)
        {
            gameSettingButton = settingButtonObj.GetComponent<Button>();
            gameSettingButton.onClick.AddListener(GameSetting);
        }

        if (quitButtonObj != null)
        {
            gameQuit = quitButtonObj.GetComponent<Button>();
            gameQuit.onClick.AddListener(GameExit);
        }

        if (goBackButtonObj != null)
        {
            gameGoBackButton = goBackButtonObj.GetComponent<Button>();
            gameGoBackButton.onClick.AddListener(GoBackButton);
        }
        yield return null;
    }
}