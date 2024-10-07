using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialsManager : MonoBehaviour
{
    public static TutorialsManager instance;

    public GameObject[] tutorialPanel; // 안내 메시지를 표시할 UI 패널
    private int textNumber = 0;

    private float timer = 0;

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

    private void Start()
    {
        for(int i = 0; i < tutorialPanel.Length; i++)
        {
            tutorialPanel[i].SetActive(false); // 시작 시 패널 비활성화
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            HideTutorial();
        }
    }

    public void ShowTutorial()
    {
        timer = 0;

        if (textNumber % 3 == 0)
        {
            tutorialPanel[0].SetActive(true); //가이드 활성화
            tutorialPanel[1].SetActive(false);
            tutorialPanel[2].SetActive(false);
        }
        else if (textNumber % 3 == 1)
        {
            tutorialPanel[0].SetActive(false); //가이드 활성화
            tutorialPanel[1].SetActive(true);
            tutorialPanel[2].SetActive(false);
        }
        else if (textNumber % 3 == 2)
        {
            tutorialPanel[0].SetActive(false); //가이드 활성화
            tutorialPanel[1].SetActive(false);
            tutorialPanel[2].SetActive(true);
        }
        textNumber++;
    }

    public void HideTutorial()
    {
        if(textNumber == 0)
        {
            tutorialPanel[textNumber % 3].SetActive(false); // 패널 비활성화
        }
        else
        {
            tutorialPanel[textNumber % 3 - 1].SetActive(false); // 패널 비활성화
        }
    }
}
