using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject pauseMenu;
    public GameObject settingMenu;

    private bool isPause = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(false);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu")
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (!isPause)
                {
                    PauseGame();
                }
                else
                {
                    ReGame();
                }
            }
        }
    }

    public void PauseGame() //일시정지 창 켜기
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        isPause = true;
        Debug.Log("PauseGame");
    }

    public void ReGame() //일시정지 창 끄기
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        isPause = false;
        Debug.Log("ReGame");
    }

    public void ContinueButton() //esc - 계속하기
    {
        ReGame();
    }

    public void SettingButton() //esc - 설정
    {
        pauseMenu.SetActive(false);
        settingMenu.SetActive(true);
        Debug.Log("설정");
    }

    public void ExitGameButton() //esc - 게임종료
    {
        pauseMenu.SetActive(false);
        isPause = false;
        Time.timeScale = 1;
        Debug.Log("QuitGameButton");
        Application.Quit();
    }

    public void GoBackButton()
    {
        pauseMenu.SetActive(true);
        settingMenu.SetActive(false);
        Debug.Log("뒤로 가기");
    }
}