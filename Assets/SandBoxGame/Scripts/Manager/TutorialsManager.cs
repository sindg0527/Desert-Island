using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialsManager : MonoBehaviour
{
    public static TutorialsManager instance;

    public GameObject[] tutorialPanel; // �ȳ� �޽����� ǥ���� UI �г�
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
            tutorialPanel[i].SetActive(false); // ���� �� �г� ��Ȱ��ȭ
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
            tutorialPanel[0].SetActive(true); //���̵� Ȱ��ȭ
            tutorialPanel[1].SetActive(false);
            tutorialPanel[2].SetActive(false);
        }
        else if (textNumber % 3 == 1)
        {
            tutorialPanel[0].SetActive(false); //���̵� Ȱ��ȭ
            tutorialPanel[1].SetActive(true);
            tutorialPanel[2].SetActive(false);
        }
        else if (textNumber % 3 == 2)
        {
            tutorialPanel[0].SetActive(false); //���̵� Ȱ��ȭ
            tutorialPanel[1].SetActive(false);
            tutorialPanel[2].SetActive(true);
        }
        textNumber++;
    }

    public void HideTutorial()
    {
        if(textNumber == 0)
        {
            tutorialPanel[textNumber % 3].SetActive(false); // �г� ��Ȱ��ȭ
        }
        else
        {
            tutorialPanel[textNumber % 3 - 1].SetActive(false); // �г� ��Ȱ��ȭ
        }
    }
}
